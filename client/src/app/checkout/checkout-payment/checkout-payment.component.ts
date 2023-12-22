import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BasketService } from 'src/app/basket/basket.service';
import { CheckoutService } from '../checkout.service';
import { ToastrService } from 'ngx-toastr';
import { Basket } from 'src/app/shared/models/basket';
import { Address } from 'src/app/shared/models/user';
import { NavigationExtras, Route, Router } from '@angular/router';
import { Stripe, StripeCardCvcElement, StripeCardExpiryElement, StripeCardNumberElement, loadStripe } from '@stripe/stripe-js';
import { OrderToCreate } from 'src/app/shared/models/order';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-checkout-payment',
  templateUrl: './checkout-payment.component.html',
  styleUrls: ['./checkout-payment.component.scss']
})
export class CheckoutPaymentComponent implements OnInit {
  @Input() checkoutForm?: FormGroup;

  @ViewChild('cardNumber') cardNumberElement?: ElementRef;
  @ViewChild('cardExpiry') cardExpiryElement?: ElementRef;
  @ViewChild('cardCvc') cardCvcElement?: ElementRef;

  stripe: Stripe | null = null;
  cardNumber?: StripeCardNumberElement;
  cardExpiry?: StripeCardExpiryElement;
  cardCvc?: StripeCardCvcElement;
  cardNumberCompleted = false;
  cardExpiryCompleted = false;
  cardCvcCompleted = false;
  cardErrors: any;
  loading = false;

  constructor(private basketService: BasketService, private checkoutService: CheckoutService, 
    private toastr: ToastrService, private router:Router) { }

  ngOnInit(): void {
    loadStripe('pk_test_51ONQWoDeOm1R34At6wljb3GdDzHs9yFLv04Ms1pUpz3KsH64dAmPWw37xBflVxmruWwDx4eudslXRrGHIlB3N7pA00BfyddWZA').then( stripe => {
      this.stripe = stripe;
      const elements = stripe?.elements();
      if(elements){
        this.cardNumber = elements.create('cardNumber');
        this.cardNumber.mount(this.cardNumberElement?.nativeElement);
        this.cardNumber.on('change', event => {
          this.cardNumberCompleted = event.complete;
          if(event.error){
            this.cardErrors = event.error.message;
          }else{
            this.cardErrors = null;
          }
        });

        this.cardExpiry = elements.create('cardExpiry');
        this.cardExpiry.mount(this.cardExpiryElement?.nativeElement);
        this.cardExpiry.on('change', event => {
          this.cardExpiryCompleted = event.complete;
          if(event.error){
            this.cardErrors = event.error.message;
          }else{
            this.cardErrors = null;
          }
        });

        this.cardCvc = elements.create('cardCvc');
        this.cardCvc.mount(this.cardCvcElement?.nativeElement);
        this.cardCvc.on('change', event => {
          this.cardCvcCompleted = event.complete;
          if(event.error){
            this.cardErrors = event.error.message;
          }else{
            this.cardErrors = null;
          }
        });
      }
    })
  }

  async submitOrder() {
    this.loading = true;
    const basket = this.basketService.getCurrentBasketValue();
    if(!basket) throw new Error("cannot get basket");
    
    try {
      const createOrder = await this.createOrder(basket);
      const paymentResult = await this.confirmPaymentWithStripe(basket);

      if(paymentResult.paymentIntent){
        this.basketService.deleteLocalBasket();
        
        const navigationExtras: NavigationExtras = {state: createOrder}; 
        this.router.navigate(['checkout/success'], navigationExtras);
      }else{
        this.toastr.error(paymentResult.error.message);
      }

    } catch (error: any) {
      console.log(error);
      this.toastr.error(error.message)
    } finally {
      this.loading = false;
    }
  }

  private async confirmPaymentWithStripe(basket: Basket | null) {
    if(!basket) throw new Error('Basket is null');

    const result = this.stripe?.confirmCardPayment(basket.clientSecret!, {
      payment_method:{
        card: this.cardNumber!,
        billing_details: {
          name: this.checkoutForm?.get('paymentForm')?.get('nameOnCard')?.value
        }
      }
    });

    if(!result) throw new Error('Problem attempting payment with stripe');

    return result;
  }

  private async createOrder(basket: Basket | null) {
    if(!basket) throw new Error('Basket is null');

    const orderToCreate = this.getOrderTpoCretate(basket);
    return firstValueFrom(this.checkoutService.createOrder(orderToCreate));
  }

  getOrderTpoCretate(basket: Basket):OrderToCreate {
    const deliveryMethodId = this.checkoutForm?.get('deliveryForm')?.get('deliveryMethod')?.value;
    const shipToAddress = this.checkoutForm?.get('addressForm')?.value as Address;
    if(!deliveryMethodId || !shipToAddress)  throw new Error('Problem with basket');

    return {
      basketId: basket.id,
      deliveryMethodId: deliveryMethodId,
      shipToAddress: shipToAddress
    }
  }

  get availabilityStatus () {
    return this.cardCvcCompleted 
      && this.cardExpiryCompleted 
      && this.cardNumberCompleted 
      && this.checkoutForm?.get('paymentForm')?.valid;
  }
}

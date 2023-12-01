import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, Validators, AsyncValidatorFn } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from '../account.service';
import { debounce, debounceTime, finalize, map, of, switchMap, take } from 'rxjs';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  registerForm: any;

  errors: string[] | null = null;

  passwordPatern = "(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$";
  constructor(private fb: FormBuilder, private router: Router, private accountService: AccountService) {
    this.registerForm = this.fb.group({
      displayName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email], [this.validateEmailNotToTaken()]],
      password: ['', [Validators.required, Validators.pattern(this.passwordPatern)]]
    });
  }

  onSubmit() {
    this.accountService.register(this.registerForm.value).subscribe({
      next: () => {
        this.router.navigateByUrl("/login")
      },
      error: error => {
        this.errors = error.errors
      }
    });
  }

  validateEmailNotToTaken(): AsyncValidatorFn {
    return (control: AbstractControl) => {
      return control.valueChanges.pipe(
        debounceTime(1000),
        take(1),
        switchMap( () => {
          return this.accountService.isEmailExists(control.value).pipe(
            map(result => result ? { emailExists: true } : null),
            finalize(() => control.markAsTouched())
          )
        })
      );
    }
  }
}

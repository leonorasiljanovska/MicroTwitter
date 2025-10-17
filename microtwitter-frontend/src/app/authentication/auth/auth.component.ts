import { Component } from '@angular/core';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { Router } from '@angular/router';
import { UserLoginDTO } from 'src/app/models/UserLoginDTO';
import { UserRegisterDTO } from 'src/app/models/UserRegisterDTO';
@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.scss']
})
export class AuthComponent {
isLoginMode: boolean=true;
user: UserRegisterDTO = {
   username: '',
    password: '',
    email: ''
};
  successMessage: string = '';
  errorMessage: string = '';


constructor(private authService: AuthenticationService, private router: Router){}

toggleMode(){
  this.isLoginMode=!this.isLoginMode;
  this.user={
     username: '',
    password: '',
    email: ''
  };
   this.successMessage = '';
   this.errorMessage = '';
}

submit(){

  this.successMessage = '';
    this.errorMessage = '';

  if (this.isLoginMode){  
    const loginDTO: UserLoginDTO = {
      username: this.user.username,
      password: this.user.password
    };
    
    this.authService.login(loginDTO)
      .subscribe({
        next:(result)=>{
          console.log('Login successful, token:', result.token);
          this.authService.saveToken(result.token);
          this.authService.saveUsername(this.user.username);
          // The *ngIf in app.component will automatically show posts now
          console.log('Is logged in?', this.authService.isLoggedIn());
        },
        error: (err)=> {
          console.error('Login error:', err);
          this.errorMessage = err.error || 'Login failed. Please check your credentials.';
        }
      });
  } else{
    this.authService.register(this.user)
    .subscribe({
      next: (message)=> {
        console.log('Registration successful:', message);
        this.successMessage = message;
        // Switch to login mode after 2 seconds
          setTimeout(() => {
            this.toggleMode();
          }, 2000);
        // this.toggleMode();
      },
      error: (err) => {
          console.error('Registration error:', err);
          this.errorMessage = err.error || 'Registration failed. Please try again.';
        }
    });
  }
}
}

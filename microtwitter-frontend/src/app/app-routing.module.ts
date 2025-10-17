import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PostListComponent } from './components/post-list/post-list.component';
import { AuthComponent } from './authentication/auth/auth.component';
import { AuthGuard } from './guards/auth.guard';
const routes: Routes = [
  { path: '', redirectTo: '/auth', pathMatch: 'full' },
  {path: 'auth', component: AuthComponent},
  {path: '', component: PostListComponent},
  
  
  // {path: 'login', component: LoginComponent},
  // {path: 'register', component: RegisterComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

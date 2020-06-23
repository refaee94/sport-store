import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthenticationComponent } from './authentication/authentication.component';
import { AuthenticationService } from './Authentication.service';
import { AuthenticationGuard } from './authentication.guard';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';

@NgModule({
  imports: [
    RouterModule, FormsModule, BrowserModule
  ],
  providers: [AuthenticationService, AuthenticationGuard],
  declarations: [AuthenticationComponent],
  exports: [AuthenticationComponent]
})
export class AuthModule { }

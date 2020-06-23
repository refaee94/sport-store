import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { AppComponent } from './app.component';
import { ModelModule } from './models/model.module';
import { RoutingConfig } from './app.routing';
import { StoreModule } from './store/store.module';
import { APP_BASE_HREF } from '@angular/common';
import { HashLocationStrategy, LocationStrategy } from '@angular/common';
import { AdminModule } from './admin/admin.module';
import { ErrorHandler } from '@angular/core';
import { ErrorHandlerService } from './errorHandler.service';
import { AuthModule } from './auth/auth.module';
import { JwtModule } from '@auth0/angular-jwt';
const eHandler = new ErrorHandlerService();
export function handler() {
  return eHandler;
}
export function tokenGetter() {
  return localStorage.getItem('token');
}
@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    ModelModule,
    RoutingConfig,
    StoreModule,
    AdminModule,
    AuthModule,
    JwtModule.forRoot({
      config: {
        tokenGetter,
        whitelistedDomains: ['localhost:5000']
      }
    })
  ],
  providers: [
    // { provide: APP_BASE_HREF, useValue: '/' },
     { provide: LocationStrategy, useClass: HashLocationStrategy },
    { provide: ErrorHandlerService, useFactory: handler },
    { provide: ErrorHandler, useFactory: handler }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}

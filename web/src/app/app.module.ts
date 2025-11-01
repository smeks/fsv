import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatNativeDateModule } from '@angular/material/core';
import { BrowserModule } from '@angular/platform-browser';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './material/material.module';
import { MAT_FORM_FIELD_DEFAULT_OPTIONS } from '@angular/material/form-field';
import { GoogleChartsModule } from 'angular-google-charts';

// app stuff
import { JwtInterceptor } from './_helpers/jwt.interceptor';
import { ErrorInterceptor } from './_helpers/error.interceptor';
import { HomeComponent } from './home/home.component';
import { PlayerService } from './services/player.service';
import { LoginComponent } from './login/login.component';
import { StartupService } from './services/startup-service';
import { EconomyApiClient } from './services/economy-api-client';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { AirportsComponent } from './airports/airports.component';
import { AirportService } from './services/airport-service.service';
import { MyflightComponent } from './myflight/myflight.component';
import { AircraftService } from './services/aircraft-service.service';

@NgModule({
    declarations: [
        AppComponent,
        HomeComponent,
        LoginComponent,
        AirportsComponent,
        MyflightComponent
    ],
    imports: [
        BrowserModule,
        MaterialModule,
        HttpClientModule,
        BrowserAnimationsModule,
        FormsModule,
        ReactiveFormsModule,
        MatNativeDateModule,
        GoogleChartsModule.forRoot('AIzaSyD-9tSrke72PouQMnMX-a7eZSW0jkFMBWY'),
        AppRoutingModule
    ],
    providers: [
        { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
        {
            provide: MAT_FORM_FIELD_DEFAULT_OPTIONS,
            useValue: { appearance: 'fill' }
        },
        PlayerService,
        StartupService,
        EconomyApiClient,
        AirportService,
        AircraftService
    ],
    bootstrap: [AppComponent]
})
export class AppModule {}

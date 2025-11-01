import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { StartupService } from './startup-service';
import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class EconomyApiClient {
    constructor(
        private httpClient: HttpClient,
        private startupService: StartupService
    ) {}

    get<T>(endpoint: string): Observable<T> {
        const api = this.startupService.getApiUrl();
        return this.httpClient.get<T>(`${api}/${endpoint}`);
    }

    post<T>(endpoint: string, data: any): Observable<T> {
        const api = this.startupService.getApiUrl();
        return this.httpClient.post<T>(`${api}/${endpoint}`, data);
    }

    put<T>(endpoint: string, data: any): Observable<T> {
        const api = this.startupService.getApiUrl();
        return this.httpClient.put<T>(`${api}/${endpoint}`, data);
    }

    delete<T>(endpoint: string): Observable<T> {
        const api = this.startupService.getApiUrl();
        return this.httpClient.delete<T>(`${api}/${endpoint}`);
    }
}

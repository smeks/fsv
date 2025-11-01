import { Injectable } from '@angular/core';
import { EconomyApiClient } from './economy-api-client';
import { Observable } from 'rxjs';
import { JobDTO } from '../dto/JobDTO';

@Injectable({
    providedIn: 'root'
})
export class AirportService {
    constructor(private economyApiClient: EconomyApiClient) {}

    getJobsByIcao(icao: string): Observable<JobDTO[]> {
        return this.economyApiClient.get(`job/${icao}`);
    }
}

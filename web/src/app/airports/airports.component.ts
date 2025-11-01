import { Component, OnInit } from '@angular/core';
import { JobDTO } from '../dto/JobDTO';
import { AirportService } from '../services/airport-service.service';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { AircraftService } from '../services/aircraft-service.service';
import { AircraftDTO } from '../dto/AircraftDTO';
import { PlayerDTO } from '../dto/PlayerDTO';
import { PlayerService } from '../services/player.service';

@Component({
    selector: 'app-airports',
    templateUrl: './airports.component.html',
    styleUrls: ['./airports.component.scss']
})
export class AirportsComponent implements OnInit {
    jobs: JobDTO[] = [];
    aircrafts: AircraftDTO[] = [];
    airportForm: FormGroup;
    player: PlayerDTO;

    jobColumns: string[] = [
        'fromIcao',
        'toIcao',
        'description',
        'amount',
        'type',
        'pay',
        'add'
    ];

    aircraftColumns: string[] = ['model', 'rentCost', 'rent'];

    constructor(
        private formBuilder: FormBuilder,
        private airportService: AirportService,
        private aircraftService: AircraftService,
        private playerService: PlayerService
    ) {}

    ngOnInit() {
        this.airportForm = this.formBuilder.group({
            icao: ['', Validators.required]
        });
    }

    onSubmit() {
        var icao = this.airportForm.get('icao').value;

        this.airportService.getJobsByIcao(icao).subscribe(x => {
            this.jobs = x;
        });

        this.aircraftService.getAircraftByIcao(icao).subscribe(x => {
            this.aircrafts = x;
        });

        this.playerService.getPlayer().subscribe(x => {
            this.player = x;
        });
    }

    rentAircraft(id: string) {
        this.playerService.rent(id).subscribe(x => {
            this.player = x;
        });
    }

    addJob(id: string) {
        this.playerService.addJob(id).subscribe(x => {
            this.player = x;

            var icao = this.airportForm.get('icao').value;
            this.airportService.getJobsByIcao(icao).subscribe(x => {
                this.jobs = x;
            });
        });
    }

    isRentingAircraft(id: string): boolean {
        if (this.player == null || this.player.rentedAircraft == null)
            return false;

        return this.player.rentedAircraft.id == id;
    }
}

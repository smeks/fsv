import { Component, OnInit } from '@angular/core';
import { PlayerDTO } from '../dto/PlayerDTO';
import { PlayerService } from '../services/player.service';
import {
    ScriptLoaderService,
    GoogleChartPackagesHelper
} from 'angular-google-charts';
import { AircraftDTO } from '../dto/AircraftDTO';

@Component({
    selector: 'app-myflight',
    templateUrl: './myflight.component.html',
    styleUrls: ['./myflight.component.scss']
})
export class MyflightComponent implements OnInit {
    player: PlayerDTO = new PlayerDTO();
    aircraft: AircraftDTO[] = [];

    displayedColumns: string[] = [
        'fromIcao',
        'toIcao',
        'description',
        'amount',
        'weight',
        'type',
        'status',
        'remove'
    ];

    aircraftDisplayedColumns: string[] = [
        'image',
        'model',
        'location',
        'crew',
        'seats',
        'rentCostPerHour',
        'weight',
        'fuel',
        'release'
    ];

    myData: google.visualization.DataTable;
    options: any;

    constructor(
        private playerService: PlayerService,
        private loaderService: ScriptLoaderService
    ) {
        this.playerService.getPlayer().subscribe(x => {
            this.player = x;
            this.aircraft = [];
            if (x.rentedAircraft != null) this.aircraft.push(x.rentedAircraft);
        });
    }

    removeJob(id: string) {
        this.playerService.removeJob(id).subscribe(x => {
            this.player = x;
        });
    }

    releaseAircraft(id: string) {
        this.playerService.release(id).subscribe(x => {
            this.player = x;
            this.aircraft = [];
        });
    }

    jobStatus(id: string): string {
        let status = 'Idle';

        if (
            this.player != null &&
            this.player.departingJobs != null &&
            this.player.departingJobs.find(x => x.id == id) != null
        )
            status = 'Departing';

        if (
            this.player != null &&
            this.player.enrouteJobs != null &&
            this.player.enrouteJobs.find(x => x.id == id) != null
        )
            status = 'Enroute';

        return status;
    }

    ngOnInit() {
        this.loaderService.onReady.subscribe(() => {
            const type = GoogleChartPackagesHelper.getPackageForChartName(
                'GeoChart'
            );
            this.loaderService.loadChartPackages([type]).subscribe(() => {
                this.myData = new google.visualization.DataTable();
                this.myData.addColumn('number', 'Lat');
                this.myData.addColumn('number', 'Long');
                // this.myData.addRows([[27.9755, -82.5332]]);

                this.options = {
                    colorAxis: {
                        minValue: 0,
                        maxValue: 0,
                        colors: ['#6699CC']
                    },
                    legend: 'none',
                    backgroundColor: {
                        fill: 'transparent',
                        stroke: '#FFF',
                        strokeWidth: 0
                    },
                    datalessRegionColor: '#f5f5f5',
                    displayMode: 'markers',
                    enableRegionInteractivity: 'true',
                    resolution: 'countries',
                    sizeAxis: {
                        minValue: 1,
                        maxValue: 1,
                        minSize: 5,
                        maxSize: 5
                    },
                    region: 'world',
                    keepAspectRatio: true,
                    width: 1000,
                    height: 500,
                    tooltip: { textStyle: { color: '#444444' } }
                };
            });
        });
    }
}

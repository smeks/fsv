import { Component } from '@angular/core';
import { AuthenticationService } from './services/authentication.service';
import { PlayerDTO } from './dto/PlayerDTO';
import { PlayerService } from './services/player.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent {
    title = 'economy-web';

    loggedIn: boolean;
    userName: string;

    playerMoney: number;

    constructor(
        private authService: AuthenticationService,
        private playerService: PlayerService
    ) {
        this.authService.userLoggedInChanged.subscribe(x => {
            this.loggedIn = x;
            if (this.loggedIn) {
                this.userName = authService.currentUser.username;
                this.playerService.getPlayer().subscribe(x => {
                    this.playerMoney = x.money;
                });
            }
        });
    }

    logout() {
        this.authService.logout();
    }
}

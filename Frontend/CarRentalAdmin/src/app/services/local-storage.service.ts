import { Injectable } from '@angular/core';

import { AuthResponse } from '../../../../../../../../Project/CarRentalForFrontendDevelopment/Frontend/CarRentalAdmin/src/app/_models/auth-responce';

@Injectable({
    providedIn: 'root'
})
export class LocalStorageService {
    private readonly accessTokenKey: string = 'accessToken';
    private readonly refreshTokenKey: string = 'refreshToken';

    constructor() {
    }

    public getAccessToken(): string | null {
        return localStorage.getItem(this.accessTokenKey);
    }

    public getRefreshToken(): string | null {
        return localStorage.getItem(this.refreshTokenKey);
    }

    public setTokens(tokenPair: AuthResponse): void {
        localStorage.setItem(this.accessTokenKey, tokenPair.accessToken);
        localStorage.setItem(this.refreshTokenKey, tokenPair.refreshToken);
    }

    public removeTokens(): void {
        localStorage.removeItem(this.accessTokenKey);
        localStorage.removeItem(this.refreshTokenKey);
    }
}
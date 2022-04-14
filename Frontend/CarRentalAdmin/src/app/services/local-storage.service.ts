import { Injectable } from '@angular/core';

import { AuthResponse } from '../models/responses/auth-response';

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

    public setTokens(authResponse: AuthResponse): void {
        localStorage.setItem(this.accessTokenKey, authResponse.accessToken);
        localStorage.setItem(this.refreshTokenKey, authResponse.refreshToken);
    }

    public removeTokens(): void {
        localStorage.removeItem(this.accessTokenKey);
        localStorage.removeItem(this.refreshTokenKey);
    }

    public hasTokens(): boolean {
        return localStorage.hasOwnProperty(this.accessTokenKey)
            && localStorage.hasOwnProperty(this.refreshTokenKey);
    }
}
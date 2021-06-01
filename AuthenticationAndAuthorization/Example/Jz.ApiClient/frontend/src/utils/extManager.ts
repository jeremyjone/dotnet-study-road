import { UserManager } from "oidc-client";

export class AppUserManager extends UserManager {
  constructor() {
    super({
      authority: "https://localhost:5001",
      client_id: "implicit client",
      redirect_uri: "http://localhost:8080/callback",
      silent_redirect_uri: "http://localhost:8080/callback",
      accessTokenExpiringNotificationTime: 3,
      silentRequestTimeout: 2000,
      response_type: "id_token token",
      scope:
        "openid profile email api.read api.create api.update api.delete mvc.delete",
      post_logout_redirect_uri: "http://localhost:8080/logout",
      filterProtocolClaims: true
    });
  }

  async login() {
    await this.signinRedirect();
    const user = await this.getUser();
    console.log(user);
    return user;
  }

  async logout() {
    return this.signoutRedirect();
  }
}

export const manager = new AppUserManager();

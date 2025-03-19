# Duende IdentityServer OAuth 2.0 & OpenID Connect API Reference

## 1. Discovery Endpoint (`/.well-known/openid-configuration`)
**Method:** `GET`

**Description:** Returns metadata containing OpenID Connect configuration.

**Parameters:** None

---

## 2. Authorization Endpoint (`/connect/authorize`)
**Method:** `GET`

**Description:** Starts the authentication and authorization process.

**Parameters:**

| Parameter | Required | Description |
|-----------|----------|-------------|
| `client_id` | ✅ | Client application ID |
| `response_type` | ✅ | Response type (`code`, `token`, `id_token`) |
| `redirect_uri` | ✅ | URL to redirect after authentication |
| `scope` | ✅ | Permissions requested (`openid profile email`) |
| `state` | ⭕ | CSRF protection token |
| `nonce` | ⭕ | Used to mitigate replay attacks |
| `code_challenge` | ⭕ | PKCE code challenge |
| `code_challenge_method` | ⭕ | Challenge method (`S256`, `plain`) |
| `login_hint` | ⭕ | Suggested username/email |
| `prompt` | ⭕ | `none`, `login`, `consent`, `select_account` |
| `max_age` | ⭕ | Maximum session age |

**Example:**
```http
GET /connect/authorize?client_id=my-client&response_type=code&redirect_uri=https://my-app.com/callback&scope=openid profile email&state=abc123&nonce=xyz456&code_challenge=xyz123&code_challenge_method=S256
```

---

## 3. Token Endpoint (`/connect/token`)
**Method:** `POST`

**Description:** Exchanges authorization code for tokens.

**Parameters:**

| Parameter | Required | Description |
|-----------|----------|-------------|
| `grant_type` | ✅ | Grant type (`authorization_code`, `password`, `client_credentials`, `refresh_token`) |
| `client_id` | ✅ | Client application ID |
| `client_secret` | ⭕ | Client secret (not required for PKCE) |
| `code` | ⭕ | Authorization code (for `authorization_code` flow) |
| `redirect_uri` | ⭕ | Required for `authorization_code` grant |
| `code_verifier` | ⭕ | PKCE verifier (if using PKCE) |
| `refresh_token` | ⭕ | Refresh token (for `refresh_token` grant) |
| `username` | ⭕ | Username (for `password` grant) |
| `password` | ⭕ | Password (for `password` grant) |
| `scope` | ⭕ | Scope (only for `client_credentials` grant) |

**Example:**
```http
POST /connect/token
Content-Type: application/x-www-form-urlencoded

grant_type=authorization_code&client_id=my-client&client_secret=my-secret&code=auth-code&redirect_uri=https://my-app.com/callback&code_verifier=xyz123
```

---

## 4. UserInfo Endpoint (`/connect/userinfo`)
**Method:** `GET`

**Description:** Retrieves user information.

**Example:**
```http
GET /connect/userinfo
Authorization: Bearer your-access-token
```

---

## 5. Introspection Endpoint (`/connect/introspect`)
**Method:** `POST`

**Description:** Validates an access or refresh token.

**Parameters:**

| Parameter | Required | Description |
|-----------|----------|-------------|
| `token` | ✅ | Token to check |
| `token_type_hint` | ⭕ | `access_token` or `refresh_token` |

**Example:**
```http
POST /connect/introspect
Content-Type: application/x-www-form-urlencoded
Authorization: Basic base64(client_id:client_secret)

token=your-access-token
```

---

## 6. Revocation Endpoint (`/connect/revocation`)
**Method:** `POST`

**Description:** Revokes an access or refresh token.

**Parameters:**

| Parameter | Required | Description |
|-----------|----------|-------------|
| `token` | ✅ | Token to revoke |
| `token_type_hint` | ⭕ | `access_token` or `refresh_token` |

**Example:**
```http
POST /connect/revocation
Content-Type: application/x-www-form-urlencoded

token=your-refresh-token&token_type_hint=refresh_token
```

---

## 7. Logout Endpoint (`/connect/endsession`)
**Method:** `GET`

**Description:** Logs the user out of IdentityServer.

**Parameters:**

| Parameter | Required | Description |
|-----------|----------|-------------|
| `id_token_hint` | ⭕ | Last ID token to identify the user |
| `post_logout_redirect_uri` | ⭕ | Redirect URL after logout |
| `state` | ⭕ | Preserves state on redirect |

**Example:**
```http
GET /connect/endsession?id_token_hint=your-id-token&post_logout_redirect_uri=https://my-app.com/logout-callback
```

---

## 8. Device Authorization Endpoint (`/connect/deviceauthorization`)
**Method:** `POST`

**Description:** Initiates OAuth2 Device Flow authentication.

**Parameters:**

| Parameter | Required | Description |
|-----------|----------|-------------|
| `client_id` | ✅ | Client application ID |
| `scope` | ✅ | Requested permissions |

**Example:**
```http
POST /connect/deviceauthorization
Content-Type: application/x-www-form-urlencoded

client_id=my-client&scope=openid profile email
```

**Response:**
```json
{
  "device_code": "your-device-code",
  "user_code": "123456",
  "verification_uri": "https://your-identityserver/device",
  "expires_in": 600
}
```

---

## Summary of API Parameters
| API | Key Parameters |
|-----|---------------|
| `/.well-known/openid-configuration` | None |
| `/connect/authorize` | `client_id`, `response_type`, `redirect_uri`, `scope`, `state`, `code_challenge` |
| `/connect/token` | `grant_type`, `client_id`, `client_secret`, `code`, `refresh_token`, `code_verifier` |
| `/connect/userinfo` | Requires access token |
| `/connect/introspect` | `token`, `token_type_hint` |
| `/connect/revocation` | `token`, `token_type_hint` |
| `/connect/endsession` | `id_token_hint`, `post_logout_redirect_uri` |
| `/connect/deviceauthorization` | `client_id`, `scope` |

This document provides a detailed overview of Duende IdentityServer's OAuth 2.0 and OpenID Connect API endpoints. If you need further assistance, feel free to ask! 🚀

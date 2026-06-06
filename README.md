# CustomerPortal
This is the customer portal repo for my APDS7311 POE Task 2


## Authentication
Authentication is implemeneted using ASP.NET Identity.
Passwords are securely stored using hashing and salting. 
That ensures that even if the database is compromised, user passwords cannot be read.
Account lockout is implemented after multiple failed login attempts to protect against brute force attacks.

## Payment System
Once logged in, users can create international payments by entering an amount, currency, account number, and SWIFT code.
Each payment is securely linked to the logged-in user and stored in a SQL Server database using Entity Framework.

## Input Validation
Server-side validation and regular expressions is used to whitelist valid input.
Invalid input is rejected to prevent malicious data from entering the system.

The application is protected against attacks such as:
## Brute Force
ASP.NET Identity lockout:
- Max 5 failed attempts
- 5-minute lockout period

Rate limiting: 
- Limits repeated login/API requests

## Injection Attacks (SQL Injection)
- Entity Framework Core (ORM) used: prevents raw SQL injection 
- Parameterized queries handled automatically by EF Core
- No direct SQL string concatenation

## Cross-Site Scripting (XSS)
- Content-Security-Policy restricts scripts to 'self'
- X-XSS-Protection enabled
- Cookies set as HttpOnly (cannot be accessed via JavaScript)

## Session Hijacking
Cookies configured as: 
- HttpOnly: not accessible via JavaScript 
- Secure: only sent over HTTPS

HTTPS enforced: encrypts session data 

## Man-in-the-Middle (MITM)
HTTPS enforced using: 
- UseHttpsRedirection()
- HSTS (Strict-Transport-Security)

TLS encryption protects all data in transit
Secure cookies prevent leakage over HTTP
# WebCinema - Online Movie Ticket Booking

A modern web application for booking movie tickets online, built with Angular and ASP.NET Core.

## Features

- Browse and search for movies
- Select showtimes and book tickets
- Choose your seats and view pricing
- Secure authentication with JWT
- Responsive UI optimized for desktop

## Tech Stack

- **Frontend:** Angular
- **Backend:** ASP.NET Core
- **Database:** SQL Server
- **Authentication:** JWT

## Getting Started

### Backend Setup

1. Open the backend project folder

2. Create a new migration (replace `migration_name` with a descriptive name):
   ```bash
   Add-Migration "migration_name"
   ```

3. Apply the migration to update the database

4. Run the backend server to expose the API endpoints

### Frontend Setup

1. Open the frontend project folder

2. Install the required npm packages:

   **If you have Angular v20 installed:**
   ```bash
   npm install
   ```

   **If you do not have Angular v20 or face dependency conflicts:**
   ```bash
   npm install --legacy-peer-deps
   ```

3. Start the frontend development server:
   ```bash
   ng serve
   ```

The frontend app will now be running and connected to the backend API.

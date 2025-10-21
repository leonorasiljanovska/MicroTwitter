# MicroTwitter

MicroTwitter is a full-stack social media project built with **.NET 9 Web API** (backend) and **Angular 17** (frontend). Users can create posts, view all posts, and see only their own posts. Each post can include an optional image URL.

## Features

- User authentication
- Create posts with text (12–140 characters)
- Optional post images (via URL)
- View all posts in a feed
- View only your own posts (filtered)
- Delete your own posts
- Frontend built in Angular 17
- Backend built in .NET 9 Web API
- Unit tests for backend

## Folder Structure

MicroTwitter/
│
├─ MicroTwitter.Api/ # .NET 9 Web API backend
├─ MicroTwitter.Tests/ # Backend unit tests
├─ microtwitter-frontend/ # Angular frontend
├─ MicroTwitter.sln # Visual Studio solution file
└─ .gitignore

## How to Run the Application

- Backend (.NET 9 Web API)

Open the solution file MicroTwitter.sln in Visual Studio 2022 (or later).

In the Solution Explorer, right-click on MicroTwitter.Api and choose “Set as Startup Project.”

The project uses an InMemory database, so no database setup or connection string configuration is needed.

Run the API using Ctrl + F5 or the Run button.

The API will start at a URL like:

https://localhost:5001


You can test the endpoints in Swagger UI, available at:

https://localhost:5001/swagger


- Frontend (Angular 17)

Open a terminal and navigate to the frontend folder:

cd microtwitter-frontend


Install dependencies:

npm install


Check the backend port:
When the .NET API starts, Visual Studio will show the port in the console (for example, https://localhost:7246).
Open your Angular service file (for example, src/app/services/auth.service.ts) and update the backend URL if needed:

private apiUrl = "https://localhost:[YOUR_PORT]/api/authentication";


Example:

private apiUrl = "https://localhost:7246/api/authentication";


Start the Angular development server:

ng serve


Open your browser and visit:

http://localhost:4200


The frontend will now connect to your backend at the specified port.

# Exchange
This project has been developed like a microservice architecture but just include one service for now. The project needs PostgreSQL, HangFire and .Net 5.0. But don't worry about installment steps, there is no necessary to install all parts of these. This project needs only one requirement which is [Docker Desktop](https://www.docker.com/products/docker-desktop).

## Tech
- .Net 5.0
- PostgreSQL
- Hangfire

## Build with Docker
[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://travis-ci.org/joemccann/dillinger)
- First of all we need to go `docker-compose.yml` 's path on our console.

     ![image](https://user-images.githubusercontent.com/38660944/138569382-bd9c5adb-cf1e-4676-9e74-3da374f45dbd.png)
     
- We can start build and install parts,  to do that we just write below command.
  ```sh
  docker-compose up --build -d 
  ```
- This build time can take some time to install some images as I said, just wait until see below picture.

     ![image](https://user-images.githubusercontent.com/38660944/138569418-c115550c-8402-4c3f-8ba8-dd1615262b68.png)

- This step provides everything what we need for our system. When containers go up, Some seed datas will add simultaneously like dummy currencies.

## About
- [PostgreSQL DB](http://localhost:36002) is running on   `localhost:36002`
- [PgAdmin for PostgreSQL](http://localhost:36003/) is running on `localhost:36003` with `username:admin@exchange` `password:admin`
- [Exchange](http://localhost:36101/swagger) is running on `localhost:36101`

## Test
  ![image](https://user-images.githubusercontent.com/38660944/138587898-987a7740-9b87-4fe1-84aa-c8c4d1a356da.png)
  - This is the first Api which return all last currencies in this system. Also we can sort fields.
---------------------
  ![image](https://user-images.githubusercontent.com/38660944/138587913-89cfcda1-790e-4df8-869a-a3d7059aee90.png)
  - The second one is return all selected code's currency day by day.
---------------------
  ![image](https://user-images.githubusercontent.com/38660944/138587932-f551a8f9-1b62-4873-8f5a-4dd12d2e23db.png)
  - The last api is an extra method. Because this method triggers beginning of hours for all day between 9am - 18pm by Hangfire. But when we need some real datas from service, we can use this api for our tests.  



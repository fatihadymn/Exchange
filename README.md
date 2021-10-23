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
- [User](http://localhost:36101/swagger) is running on `localhost:36101`

## Test
  ![image](https://user-images.githubusercontent.com/38660944/138569478-6e167dd8-112a-44ee-ac33-e04a88381f95.png)
  - This is the first Api which return all last currencies in this system. Also we can sort fields.
---------------------
  ![image](https://user-images.githubusercontent.com/38660944/138569552-fff2013e-d7c6-4a8b-b070-45298e4dd6b2.png)
  - The second one is return all selected code's currency day by day.
---------------------
  ![image](https://user-images.githubusercontent.com/38660944/138569592-d45c5a54-e832-4d04-a82f-842b6e0c86ce.png)
  - The last api is an extra method. Because this method triggers beginning of hours for all day between 9am - 18pm by Hangfire. But when we need some real datas from service, we can use this api for our tests.  



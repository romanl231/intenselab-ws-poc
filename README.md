# Realtime Chat PoC

This is a Proof-of-Concept (PoC) realtime chat system based on microservices architecture.

## Technologies

- ASP.NET Core 8 Gateway API (REST + WebSocket client)
- WebSocket microservice (standalone console service)
- Redis Pub/Sub for message broadcasting between services
- Docker + Docker Compose
- MessagePack for serialization

## Architecture

Simplified schema:

```
[Client HTTP] --> [Gateway API] --> [Redis Pub/Sub] --> [WebSocket Service] --> [Connected Clients]
                          ↑                                        ↓
                    WebSocket Client <-----------------------------
```


### Components

| Service            | Description                               |
|-------------------|-------------------------------------------|
| Gateway.API        | HTTP API for sending chat commands. Manages WebSocket connection to Realtime.WS. |
| Realtime.WS        | WebSocket server. Handles message events and broadcasts via Redis Pub/Sub. |
| Redis              | Message broker for chat events.          |

## Features

- Join / Leave / Send message events
- MessagePack-based serialization for efficiency
- Dockerized microservices
- Simple architecture for educational purposes

## Run locally

Requirements:

- .NET 8 SDK
- Docker + Docker Compose

Steps:

1. Clone the repo

```bash
git clone https://github.com/your_username/intenselab-ws-poc.git
cd intenselab-ws-poc
```

2. Run with docker compose
  ```bash
  docker-compose up --build
  ```

  This will start:
  
  - Redis
  
  - Realtime.WS (WebSocket server)
  
  - Gateway.API

3. Test the system

- Use Postman to send HTTP requests to Gateway.API.

- Use WebSocket client (e.g., wscat, Postman WS tab) to connect directly to Realtime.WS for testing.

## Example API calls
Join chat:

```http
POST /chat/send
Content-Type: application/json

{
    "sender": "John",
    "receiver": "Doe",
    "text": "string",
    "event": "string"
}
```
## Development
Project structure:

```
├── src/
│   ├── Gateway.API/       // HTTP API + WebSocket client
│   ├── Realtime.WS/       // WebSocket server microservice
│   └── Shared/            // Shared DTOs / Contracts
├── docker/
│   └── docker-compose.yml

```

## Todo / Improvements
- Add Kafka integration

- Add authentication / rooms support

- Add frontend React client

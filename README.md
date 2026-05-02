\# 🚀 LogStreamX – Real-Time Log Streaming System



\## 📌 Overview



LogStreamX is a distributed log processing system built using .NET that enables real-time ingestion, processing, and visualization of application logs.



It follows an \*\*event-driven architecture\*\* using Kafka to ensure scalability, reliability, and decoupling between services.



\---



\## 🏗️ Architecture



```

&#x20;         ┌──────────────┐

&#x20;         │   API Layer  │

&#x20;         │ (.NET Web API)

&#x20;         └──────┬───────┘

&#x20;                │

&#x20;                ▼

&#x20;         ┌──────────────┐

&#x20;         │    Kafka     │

&#x20;         │ (Message Bus)│

&#x20;         └──────┬───────┘

&#x20;                │

&#x20;                ▼

&#x20;       ┌──────────────────┐

&#x20;       │ Worker Service   │

&#x20;       │ (.NET Background)│

&#x20;       └──────┬───────────┘

&#x20;              │

&#x20;              ▼

&#x20;       ┌──────────────────┐

&#x20;       │   SQL Server     │

&#x20;       │ (Log Storage)    │

&#x20;       └──────┬───────────┘

&#x20;              │

&#x20;              ▼

&#x20;       ┌──────────────────┐

&#x20;       │   Dashboard UI   │

&#x20;       │ (Real-time view) │

&#x20;       └──────────────────┘

```



\---



\## ⚙️ Tech Stack



\* .NET 8 / .NET 10

\* ASP.NET Core Web API

\* Kafka (Confluent)

\* Background Worker Service

\* Entity Framework Core

\* SQL Server

\* SignalR (for real-time updates)



\---



\## 🔄 Flow Explanation



1\. Client sends log → API

2\. API publishes log → Kafka Topic

3\. Worker consumes messages from Kafka

4\. Worker processes \& stores logs → SQL Server

5\. Dashboard fetches \& displays logs in real-time



\---



\## ▶️ How to Run



\### 1️⃣ Start Kafka



```bash

\# Start Zookeeper

zookeeper-server-start.bat config\\zookeeper.properties



\# Start Kafka

kafka-server-start.bat config\\server.properties

```



\---



\### 2️⃣ Run API



```bash

dotnet run --project LogStreamX.API

```



\---



\### 3️⃣ Run Worker



```bash

dotnet run --project LogStreamX.Worker

```



\---



\### 4️⃣ Run Dashboard



```bash

dotnet run --project LogStreamX.Dashboard

```



\---



\## 📸 Screenshots



(Add your dashboard screenshots here)



\---



\## 💡 Key Features



\* Real-time log streaming

\* Event-driven microservice architecture

\* Fault-tolerant message processing

\* Scalable Kafka-based pipeline

\* Clean separation of concerns



\---



\## 🧠 Learnings



\* Implemented Kafka producer/consumer in .NET

\* Built background worker processing pipeline

\* Designed scalable event-driven system

\* Integrated real-time UI updates using SignalR



\---



\## 📬 Future Improvements



\* Add authentication \& authorization

\* Deploy using Docker \& Kubernetes

\* Add retry \& dead-letter queue

\* Implement centralized logging (ELK stack)



\---



\## 👨‍💻 Author



Yashasvi Saxena


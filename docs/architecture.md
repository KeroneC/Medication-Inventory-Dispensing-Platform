# Architecture Notes

## Why a monorepo?

For learning, one repository keeps the full system visible. You can trace a workflow from React, to .NET APIs, to domain rules, to test cases, to SQL and messaging artifacts without switching context.

In a production organization, these services might later move into separate repositories when teams, release schedules, and ownership boundaries justify the extra coordination cost.

## Service responsibilities

- `InventoryService` owns medication stock levels, reorder thresholds, restock actions, and low-stock detection.
- `DispensingService` owns the dispense workflow and validates required patient/requestor information.
- `AuditService` owns traceable records for compliance and troubleshooting.
- `NotificationService` owns alert delivery and acknowledgement.
- `MedicationPlatform.Common` contains shared domain records, event contracts, and demo adapters.

## Production evolution

The current version uses in-memory stores to keep the learning loop quick. A production-grade next step would add:

- SQL Server repositories for medications, inventory, dispense records, audit logs, and notifications.
- A message broker adapter for RabbitMQ, Azure Service Bus, or Kafka.
- Authentication and role-based authorization.
- OpenTelemetry tracing so one dispense request can be followed across services.
- CI checks that run unit tests, API tests, Playwright tests, and static analysis.

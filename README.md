in progress...

# EventForgingOutcomes-Sample
Purpose of this project is to show how to use:
- [EventForging](https://github.com/piotr07da/EventForging) - event sourcing library.
- [EventOutcomes](https://github.com/piotr07da/EventOutcomes) - library used to write unit tests for event sourced applications.

Beside that this project contains some solutions that someone may find useful in their own project.

This project is MIT licensed.

## Status
[![build-n-test](https://github.com/piotr07da/EventForgingOutcomes-Sample/actions/workflows/build-n-test.yml/badge.svg)](https://github.com/piotr07da/EventForgingOutcomes-Sample/actions/workflows/build-n-test.yml)

## Architecture
This project has three main layers: Application, Domain and Infrastructure.

### Application layer
Contains commands, command handlers, event handlers.

Command handlers are implemented as [MassTransit](https://masstransit.io/) message consumers. MassTransit was choosed as it:
- Provides conversation Id and initiator Id. `EventForging` takes those two identifiers and saves them to the database. Moreover initiator Id is used by `EventForging` to provide per command idempotency.
- Allows to pass additional data with every command and event. It can be used to pass expected version required by `EventForging`.
- Allows to add additional custom features as localization of domain exceptions.

### Domain layer
Contains aggregates, entities, value objects and domain events. It is based on `EventForging` library.

## Aggregate
All aggregates:
- Implement `IEventForged` interface.
- Are created using static factory methods.
- Have private void `Apply` methods for every event they or their internal entities produce. Those methods are responsible for restoring the state.

## Entity
Entities:
- Are created using static factory methods.
- Have one internal **static** void `Apply` method used to create its instance during rehydartion.
- Have internal void `Apply` methods for every event they produce. Those methods must be called by parent entity (or an aggregate) `Apply` methods.

## Value Object
Value objects can be created in two different ways:
- Using the `FromValue` method - this is used when a DTO from a command needs to be converted into a value object. During this conversion, all domain validation must be preformed to ensure the value object is valid.
- Using the `Restore` method - this method is used inside Apply mathods responsible for rehydrating an aggregate and restoring its state. Therefore, unlike the `FromValue` method, the `Restore` method cannot perform any domain validation, as the state it is restoring may no longer be consistent with current domain rules. Restoring the state of an aggregate should never fail.


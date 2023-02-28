in progress...

# EventForgingOutcomes-Sample
The purpose of this project is to show how to use the following libraries:
- [EventForging](https://github.com/piotr07da/EventForging) - used to create event sourced applications.
- [EventOutcomes](https://github.com/piotr07da/EventOutcomes) - used to write unit tests for event sourced applications.

In addition, this project includes some solutions that others may find useful in their own projects.

This project is MIT licensed.

## Status
[![build-n-test](https://github.com/piotr07da/EventForgingOutcomes-Sample/actions/workflows/build-n-test.yml/badge.svg)](https://github.com/piotr07da/EventForgingOutcomes-Sample/actions/workflows/build-n-test.yml)

## Domain
This project is yet another online store. This time it is a store of electronic components.
The example presented here has implementation of just two bounded contexts - Catalog and Sales.

Catalog context allows introduction of new products. Each product can be characterized by a set of properties.
Properties are divided into two different types - text and numeric.
Text property allows to add some textual information like type of transistor or type of capacitor.
Numeric properties are used to describe such properties like gate current, max voltage, max temeparture.
Products can also be categorized in hierarchical categories.

Sales context i responsible for introducing and pricing new products.
Products can have single unit price but can also have few different prices defined for different quantities- the higher quantity the lower the price.
Sales context is also responsible for collecting orders. Orders have order items. Each item is priced.
Discount can be added on the order level automatically or by use of promotion code.

## EventForging
The aggregates can be found in the **EFO.Sales.Domain** and **EFO.Catalog.Domain** projects.
Each aggregate has the `Events` property initialized in the constructor. This property is used inside an aggregate operations
to apply new events. State of aggregates is restored inside private `Apply` methods.

EventForging does not ... this sample project gives proposition how entities can be done. We can ...



## EventOutcomes

The first step in using the EventOutcomes library is to implement the `IAdapter` interface.
The implementation can be found in the **EFO.Shared.Tests** project.
The main purpose of implementing this component is to integrate EventOutcomes with any event sourcing framework - in our case EventForging.
Because of that, there is also a `FakeEventDatabase` implementation of the `IEventDatabase` interface from the EventForging library,
which is our key component.

Once we have implemented the `IAdapter` interface and provided a fake implementation of the `IEventDatabase` interface,
we can start writing our tests. The unit tests can be found in the **EFO.Sales.Tests** and **EFO.Catalog.Tests** projects.

The tests are build using a `Test` class, which is instantiated and prepared using the EventOutcomes fluent API.
The `Test` class can be instantiated in the constructor of testing class and shared between multiple tests,
or an instance of the `Test` class can be created inside testing methods.



## Structure
Each bounded context has following project: Application, Domain, Tests. There is also a group of Shared projects. This group does not represent any bounded context. Instead in contains shared classes resused in other projects.

### Application project
Contains commands, command handlers, event handlers.

Command handlers are implemented as [MassTransit](https://masstransit.io/) message consumers. MassTransit was choosed as it:
- Provides conversation Id and initiator Id. `EventForging` takes those two identifiers and saves them to the database. Moreover initiator Id is used by `EventForging` to provide per command idempotency.
- Allows to pass additional data with every command and event. It can be used to pass expected version required by `EventForging`.
- Allows to add additional custom features as localization of domain exceptions.

### Domain project
Contains aggregates, entities, value objects and domain events. It is based on `EventForging` library.

### Test project
Here we can find out how to use EventOutcomes library. But 

## Additional

### Aggregate
All aggregates:
- Implement `IEventForged` interface.
- Are created using static factory methods.
- Have private void `Apply` methods for every event they or their internal entities produce. Those methods are responsible for restoring the state.

### Entity
Entities:
- Are created using static factory methods.
- Have one internal **static** void `Apply` method used to create its instance during rehydartion.
- Have internal void `Apply` methods for every event they produce. Those methods must be called by parent entity (or an aggregate) `Apply` methods.

### Value Object
Value objects can be created in two different ways:
- Using the `FromValue` method - this is used when a DTO from a command needs to be converted into a value object. During this conversion, all domain validation must be preformed to ensure the value object is valid.
- Using the `Restore` method - this method is used inside Apply mathods responsible for rehydrating an aggregate and restoring its state. Therefore, unlike the `FromValue` method, the `Restore` method cannot perform any domain validation, as the state it is restoring may no longer be consistent with current domain rules. Restoring the state of an aggregate should never fail.


# Flashcards - Console App

A C# console application for creating flashcard stacks and studying them, with session tracking and monthly performance reports.

## Requirements

- [x] Two tables (stacks and flashcards) linked by a foreign key
- [x] Stacks must have a unique name
- [x] Deleting a stack cascades to its flashcards and study sessions
- [x] Flashcards are shown via a DTO, without the stack ID
- [x] Flashcard display IDs always start at 1 with no gaps, regardless of deletions
- [x] Study sessions stored with date and score
- [x] Users can view their full study session history
- [x] No update or delete operations on the study session table
- [x] **Challenge:** Report showing number of sessions per month per stack
- [x] **Challenge:** Report showing average score per month per stack

## Features

- **Stack management** - create, view, and delete stacks; names are unique enforced at both the database and service level
- **Flashcard management** - add and delete flashcards within a stack; display IDs are always sequential starting from 1
- **Study sessions** - work through a stack card by card, get immediate feedback, and have your score saved automatically
- **Session history** - view all past study sessions across all stacks
- **Monthly reports** - pivot-based reports showing sessions per month and average score per month, broken down by stack

## How It Works

On startup the app builds a menu from registered section providers and presents a main menu. You navigate into stacks, flashcards, study sessions, or reports, each backed by its own service and repository layer talking to a local SQL Server database via Dapper.

The app is built in layers:

- **Models** - domain classes (`Stack`, `Flashcard`, `StudySession`, `StackReportRow`)
- **DTOs** - `FlashCardDto` adds a sequential display index and strips the stack ID for presentation
- **Repositories** - thin Dapper wrappers over SQL; one per table plus a dedicated report repository for the pivot queries
- **Services** - business logic layer; handles duplicate name validation, the interactive study loop, and mapping to DTOs
- **Menu** - built around the Composite pattern (`IMenuItem` / `CompositeMenu`), with the Strategy pattern (`IStackAction`) powering the "select a stack, then do X" flows that appear in multiple places

## Technologies Used

- .NET 8 / C#
- SQL Server (LocalDB)
- [Dapper](https://github.com/DapperLib/Dapper) - lightweight micro-ORM; hand-written SQL, full control over queries
- [Spectre.Console](https://spectreconsole.net/) - rich terminal UI (interactive menus, styled output, tables)

## Challenges

**New patterns - Composite and Strategy**
The Composite pattern for the menu system and the Strategy pattern for stack actions (`IStackAction`) were both new. The Composite pattern in particular took time to internalise. The idea that a menu and a menu item are the same thing from the outside, and that you can nest them freely, is elegant once it clicks but not obvious going in.

**SQL PIVOT**
I knew pivot tables from Excel, so the concept was familiar, but translating it to SQL was new. Getting the syntax right and understanding how the database rotates rows into columns took some real digging. Satisfying to get working.

**The human cost of over-engineering**
This is the one I didn't expect. I went in deliberately trying to apply every SOLID principle as far as it would go, and it worked, in the sense that the code is consistent and the patterns are correctly applied. But somewhere in the middle I lost overview of my own project. The architecture became a maze I had built myself, and every new feature meant navigating layers of abstractions to figure out where the change belonged. I lost interest in finishing it. I pushed through and I'm glad I did, but it taught me something that no amount of reading about YAGNI and KISS had managed to: "correct" and "good" are not the same thing. Code that the next person, or future me, can actually follow and work in is worth more than code that ticks every best-practice box.

## What Came Easily

SQL itself felt natural throughout, including the pivot queries once the syntax was clear. Dapper continues to grow on me. The explicit control over queries is a good fit for how I think about data, and I prefer it to EF Core where the underlying SQL is largely hidden. Async/await, DTOs, and the repository pattern are all well-practised by now.

## What I Learned

- SQL `PIVOT` syntax and how it maps a concept I already knew from Excel into a query
- The Composite and Strategy patterns, what problems they solve and how to implement them
- That SOLID, taken all the way, can become its own kind of problem. The principles are sound but they exist to serve the code, not the other way around. I came into this project very much a "best practices all the way" person. I'm leaving it with more respect for KISS and YAGNI, and a better sense of when a pattern earns its place versus when it just adds weight

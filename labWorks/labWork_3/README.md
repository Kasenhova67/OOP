# Student Record Management System

## Overview

This project is a console application for managing student records with motivational quote integration. It follows clean architecture principles with separation of concerns between domain, application, infrastructure, and presentation layers.

## Project Structure

### 1. Domain Layer (Core Business Logic)
- **Entities**: Fundamental business objects
  - `Student.cs`: Represents a student with Id, Name, and Grade
  - `Quote.cs`: Represents a motivational quote with Content and Author
- **DTOs**: Data Transfer Objects
  - `StudentDTO.cs`: For transferring student data between layers
  - `QuoteDTO.cs`: For transferring quote data between layers
- **Validators**: Business rule validation
  - `StudentValidator.cs`: Validates student data using FluentValidation

### 2. Application Layer (Business Use Cases)
- **Commands**: CQRS pattern commands
  - `AddStudentCommand.cs`: Handles adding new students
  - `EditStudentCommand.cs`: Handles editing existing students
  - `ViewStudentsCommand.cs`: Handles viewing all students
- **Services**: Business logic services
  - `IStudentService.cs`: Interface for student operations
  - `StudentService.cs`: Implementation of student operations
  - `IQuoteService.cs`: Interface for quote operations
  - `QuoteService.cs`: Implementation of quote operations

### 3. Infrastructure Layer (Technical Implementation)
- **Adapters**: External system integration
  - `IQuoteApiAdapter.cs`: Interface for quote API
  - `QuoteApiAdapter.cs`: Implementation for quote API using HttpClient
- **Factories**: Object creation
  - `IStudentFactory.cs`, `StudentFactory.cs`: Student creation
  - `IQuoteFactory.cs`, `QuoteFactory.cs`: Quote creation
- **Repositories**: Data persistence
  - `IStudentRepository.cs`: Interface for student data access
  - `StudentRepository.cs`: JSON file-based implementation

### 4. Presentation Layer (Console UI)
- `ConsoleUI.cs`: Handles user interaction and menu system
- `Program.cs`: Application entry point with DI configuration

## Key Features

### Student Management
- **Add Student**:
  - Validates input (name length 2-100 chars, grade 0-100)
  - Persists to JSON file
  - Displays motivational quote after adding
- **Edit Student**:
  - Validates input
  - Updates existing record
- **View Students**:
  - Displays all students in tabular format
  - Shows ID, Name, and Grade

### Motivational Quotes
- Fetches random quotes from external API (quotable.io)
- Fallback to local quote if API unavailable
- Displays quote after adding student

### Data Persistence
- Uses JSON file storage (`students.json`)
- Maintains student IDs automatically
- Thread-safe file operations

## Technical Details

### Validation Rules
- Student Name:
  - Required
  - 2-100 characters
- Student Grade:
  - Required
  - Integer between 0-100

### External Integration
- Quote API:
  - Endpoint: `https://api.quotable.io/random`
  - Timeout: 5 seconds
  - Fallback mechanism when:
    - API unavailable
    - Network issues
    - Invalid response

### Error Handling
- Validation errors show specific messages
- API errors show fallback quote
- General errors show user-friendly messages

## How It Works

1. **Startup**:
   - Dependency Injection configures all services
   - JSON file storage initialized
   - HTTP client configured for quote API

2. **Main Loop**:
   - Displays menu options
   - Processes user input
   - Executes corresponding command

3. **Data Flow**:
   - ConsoleUI collects input → Creates DTO → Executes Command
   - Command uses Services → Services use Repositories/Factories
   - Results returned to UI for display


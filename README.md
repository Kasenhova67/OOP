# LabWork_2
# Document Editor Application 

## Overview
This is a console-based document editor application that supports various document types, formatting, and storage options. The application follows clean architecture principles and implements several design patterns.

## Key Components

### 1. Core Components

#### AppSettings (Singleton Pattern)
- **Purpose**: Manages application-wide settings
- **Features**:
  - Background/Text colors
  - Font size
  - Theme
  - Google Drive access token
- **Pattern**: Singleton - ensures only one instance exists

#### Document (Observer Pattern)
- **Purpose**: Represents a document with content
- **Features**:
  - Content management (add, insert, delete)
  - Undo/Redo functionality
  - Two display modes (Preview/Edit)
  - Observer pattern for change notifications
- **Patterns**:
  - Observer - notifies users of changes
  - Command - for undo/redo operations

#### Text Components (Decorator Pattern)
- **Classes**:
  - `TextComponent` (abstract base)
  - `PlainText` (concrete implementation)
  - `BoldText`, `ItalicText`, `UnderlineText`, `HeaderText` (decorators)
- **Pattern**: Decorator - adds formatting to text dynamically

### 2. Management Components

#### DocumentManager
- **Purpose**: Central document operations manager
- **Features**:
  - Document creation via factories
  - Loading/saving via adapters
  - Storage strategy management
- **Dependencies**:
  - Uses factories, adapters, and storage strategies

#### User Management
- **Classes**:
  - `User` (implements IObserver)
  - `UserRole` enum (Viewer, Editor, Admin)
- **Features**:
  - Role-based permissions
  - Document change notifications

### 3. Design Pattern Implementations

#### Factory Pattern
- **Purpose**: Create different document types
- **Classes**:
  - `IDocumentFactory` interface
  - `PlainTextFactory`, `MarkdownFactory`, `RichTextFactory`
- **Usage**: DocumentManager uses factories to create documents

#### Adapter Pattern
- **Purpose**: Convert between document formats
- **Classes**:
  - `IDocumentAdapter` interface
  - `PlainTextAdapter`, `JsonAdapter`, `XmlAdapter`, `MarkdownAdapter`, `RichTextAdapter`
- **Usage**: DocumentManager uses adapters for file I/O

#### Strategy Pattern
- **Purpose**: Different storage implementations
- **Classes**:
  - `IStorageStrategy` interface
  - `LocalFileStorage`, `OneDriveLocalStorage`
- **Usage**: DocumentManager uses strategies for storage operations

### 4. Main Application

#### DocumentEditorApp
- **Purpose**: Main application controller
- **Features**:
  - User login/role management
  - Menu system for operations
  - Settings management
  - Document editing interface


## Input/Output Flow

### Document Operations:
- **Input**: User commands via console menu
- **Processing**:
  - DocumentManager coordinates operations
  - Factories/Adapters/Strategies handle specific tasks
- **Output**: 
  - Console feedback
  - File operations
  - Document display

### Text Formatting:
- **Input**: Text content + format commands
- **Processing**:
  - Decorators wrap text components
  - Document maintains component list
- **Output**: Formatted text display

## Supported Features

1. **Document Types**:
   - Plain text
   - Markdown
   - Rich text

2. **File Formats**:
   - .txt (plain text)
   - .json
   - .xml
   - .md (markdown)

3. **Storage Options**:
   - Local filesystem
   - OneDrive

4. **Text Formatting**:
   - Bold
   - Italic
   - Underline
   - Headers (1-3)

5. **User Roles**:
   - Viewer (read-only)
   - Editor (can edit)
   - Admin (full access)

## Design Patterns Summary

1. **Singleton**: AppSettings
2. **Decorator**: Text formatting
3. **Factory**: Document creation
4. **Adapter**: File format conversion
5. **Strategy**: Storage implementations
6. **Observer**: Document change notifications
7. **Command**: Undo/Redo operations

The application demonstrates how these patterns can work together to create a flexible, maintainable system with clear separation of concerns.

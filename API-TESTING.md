# API Testing Guide

Base URL: `http://localhost:5120`
Swagger UI: `http://localhost:5120/swagger`

Seed data is created automatically on first run (5 books, 3 members, 2 active borrowings), so `GET` requests will return data immediately. Since seeded IDs are GUIDs generated at runtime, grab real IDs from the `GET /api/books` and `GET /api/members` responses before testing borrow/return.

## Books

### Create a book
```bash
curl -X POST http://localhost:5120/api/books \
  -H "Content-Type: application/json" \
  -d '{
    "title": "The Hobbit",
    "author": "J.R.R. Tolkien",
    "isbn": "9780618260300",
    "publishedYear": 1937,
    "totalCopies": 2
  }'
```
Expect: `201 Created` with the created `BookResponse`.

### Get all books
```bash
curl http://localhost:5120/api/books
```

### Get a book by id
```bash
curl http://localhost:5120/api/books/<book-id>
```
Expect: `200 OK`, or `404 Not Found` for an unknown id.

### Update a book
```bash
curl -X PUT http://localhost:5120/api/books/<book-id> \
  -H "Content-Type: application/json" \
  -d '{
    "title": "The Hobbit (Revised Edition)",
    "author": "J.R.R. Tolkien",
    "isbn": "9780618260300",
    "publishedYear": 1937,
    "totalCopies": 3
  }'
```

### Delete a book
```bash
curl -X DELETE http://localhost:5120/api/books/<book-id>
```
Expect: `204 No Content`.

### Validation / error cases to try
- `POST /api/books` with a duplicate `isbn` → `409 Conflict`
- `POST /api/books` with `publishedYear` in the future → `400 Bad Request`
- `POST /api/books` with `totalCopies: 0` → `400 Bad Request`
- `POST /api/books` missing `title` → `400 Bad Request` with a validation error list

## Members

### Register a member
```bash
curl -X POST http://localhost:5120/api/members \
  -H "Content-Type: application/json" \
  -d '{
    "fullName": "Dana Lee",
    "email": "dana.lee@example.com",
    "phoneNumber": "0712345678"
  }'
```
Expect: `201 Created`, `isActive: true` by default.

### Get all members
```bash
curl http://localhost:5120/api/members
```

### Get a member by id
```bash
curl http://localhost:5120/api/members/<member-id>
```

### Update a member
```bash
curl -X PUT http://localhost:5120/api/members/<member-id> \
  -H "Content-Type: application/json" \
  -d '{
    "fullName": "Dana Lee",
    "email": "dana.lee@example.com",
    "phoneNumber": "0712345678",
    "isActive": false
  }'
```

### Delete a member
```bash
curl -X DELETE http://localhost:5120/api/members/<member-id>
```

### Validation / error cases to try
- `POST /api/members` with a duplicate `email` → `409 Conflict`
- `POST /api/members` with an invalid email format → `400 Bad Request`
- `POST /api/members` missing `fullName` → `400 Bad Request`

## Borrowings

### Borrow a book
```bash
curl -X POST http://localhost:5120/api/borrowings \
  -H "Content-Type: application/json" \
  -d '{
    "bookId": "<book-id>",
    "memberId": "<member-id>"
  }'
```
Expect: `201 Created` with `status: "Borrowed"` and a `dueDate` 14 days out.

### Get all borrowings
```bash
curl http://localhost:5120/api/borrowings
```

### Get a member's borrowing history
```bash
curl http://localhost:5120/api/members/<member-id>/borrowings
```

### Return a book
```bash
curl -X POST http://localhost:5120/api/borrowings/<borrowing-id>/return
```
Expect: `200 OK` with `status: "Returned"` and `returnedDate` set.

### Business rule cases to try
- Borrow a book whose `availableCopies` is `0` → `400 Bad Request`
- Borrow with an inactive member (set `isActive: false` via the member update endpoint first) → `400 Bad Request`
- Borrow a 4th book for the same member (limit is 3 active borrowings) → `400 Bad Request`
- Call return twice on the same borrowing id → `400 Bad Request` the second time
- Any request with an unknown `bookId`/`memberId`/borrowing id → `404 Not Found`

## Quick end-to-end flow

1. `GET /api/books` and `GET /api/members` — copy a seeded `id` from each.
2. `POST /api/borrowings` with those ids — note the returned borrowing `id`.
3. `GET /api/books/<book-id>` — confirm `availableCopies` dropped by 1.
4. `POST /api/borrowings/<borrowing-id>/return` — confirm status changes to `Returned`.
5. `GET /api/books/<book-id>` again — confirm `availableCopies` went back up by 1.

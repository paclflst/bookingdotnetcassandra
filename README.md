.NET application for hotel rooms booking which interacts with the Cassandra database through RESTful API.

Functional Requirements

 Support of the multi-threading data processing.
 Asynchronous execution of database queries through RESTful API implementing futures.
 RESTful API with the JSON data representation.
RESTful API Queries

POST URL: "api" / "get" / "city"

JSON example: {"city":"London"}

Explanation: Get all hotels of a current city.

POST URL: "api" / "add" / "hotel"

Explanation: Adds a new hotel to the system.

POST URL: "api" / "add" / "guest"

Explanation: Adds a new guest, who is going to book a room.

POST URL: "api" / "add" / "room"

Explanation: Adds a new room to a hotel.

POST URL: "api" / "get" / "freerooms"

Explanation: Gets free rooms in a specific hotel for the current period.

For example - "startReserveTime":"2016/4/21","endReserveTime":"2016/4/23"

POST URL: "api" / "get" / "roombyguest"

Explanation: Guest has booked some room / rooms. Gets booked room / rooms by the specific date and guest number.

"api" / "add" / "booking"

Explanation: Adds a new room booking for a guest.

Checks if the room is available when adding a new booking to a Cassandra table.

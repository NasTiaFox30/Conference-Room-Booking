-- Test Conference Rooms
INSERT INTO ConferenceRooms (RoomName, Capacity, Location, Description, HasProjector, HasWhiteboard, HasAudioSystem, HasWiFi)
VALUES 
('Sala A - Mała sala konferencyjna', 10, '1 piętro, pok. 101', 'Mały pokój do spotkań zespołowych i dyskusji', 1, 1, 0, 1),
('Sala B - Średnia sala konferencyjna', 20, '2 piętro, pok. 205', 'Średnia sala konferencyjna z telewizorem i systemem audio', 1, 1, 1, 1),
('Sala Konferencyjna "Centralna"', 50, '3 piętro, pok. 301', 'Duża sala konferencyjna do prezentacji i wydarzeń firmowych', 1, 1, 1, 1),
('Sala szkoleniowa', 30, '2 piętro, pok. 210', 'Sala do prowadzenia szkoleń i wydarzeń edukacyjnych', 1, 1, 0, 1),
('Przestrzeń kreatywna', 15, '1 piętro, pok. 115', 'Nieformalna przestrzeń do burzy mózgów i sesji kreatywnych', 0, 1, 0, 1);
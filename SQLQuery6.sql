SELECT Surf.spot_name, forecast_for_date, Destination.name,
swell_height_feet, wind_direction, Flight.price, Destination.latitude, 
Flight.origin_airport_code, Destination.airport_code, Destination.longitude, 
Flight.departure_date, Flight.flight_id, Flight.return_date, 
(SELECT AVG(Surf.swell_height_feet) FROM Surf WHERE Surf.spot_name = 'Tavarua-Cloudbreak') AS historical_average_surf, 
(SELECT MAX(Surf.swell_height_feet) FROM Surf WHERE Surf.spot_name = 'Tavarua-Cloudbreak') AS historical_max_surf, 
(SELECT MIN(Surf.swell_height_feet) FROM Surf WHERE Surf.spot_name = 'Tavarua-Cloudbreak') AS historical_min_surf, 
(SELECT AVG(Flight.price), Surf.spot_name FROM Flight 
JOIN Destination ON Destination.airport_code = Flight.airport_code
JOIN Surf ON Destination.location_id = Surf.location_id
WHERE Surf.spot_name = 'Uluwatu'
GROUP BY spot_name) AS historical_average_price, 
(SELECT MIN(Flight.price) FROM Flight
JOIN Destination ON Destination.airport_code = Flight.airport_code
JOIN Surf ON Destination.location_id = Surf.location_id
WHERE Surf.spot_name = 'Tavarua-Cloudbreak') AS historical_min_price, 
(SELECT MAX(Flight.price) FROM Flight
JOIN Destination ON Destination.airport_code = Flight.airport_code
JOIN Surf ON Destination.location_id = Surf.location_id
WHERE Surf.spot_name = 'Tavarua-Cloudbreak') AS historical_max_price FROM Surf
JOIN Destination ON Destination.location_id = Surf.location_id
JOIN Flight ON Destination.location_id = Surf.location_id
WHERE Surf.spot_name = 'Tavarua-Cloudbreak' AND 
Flight.flight_id = 2182 AND log_id IN
(SELECT TOP 80 log_id FROM SURF ORDER BY log_id DESC)
GROUP BY Surf.spot_name, forecast_for_date, Destination.name,
swell_height_feet, wind_direction, Flight.price, Destination.latitude, 
Flight.origin_airport_code, Destination.airport_code, Destination.longitude, 
Flight.departure_date, Flight.flight_id, Flight.return_date
ORDER BY forecast_for_date ASC;


SELECT AVG(Flight.price), Surf.spot_name FROM Flight 
JOIN Destination ON Destination.airport_code = Flight.airport_code
JOIN Surf ON Destination.location_id = Surf.location_id
WHERE Surf.spot_name = 'Uluwatu'
GROUP BY spot_name
SELECT Surf.spot_name, forecast_for_date, Destination.name,
swell_height_feet, wind_direction, Flight.price, Destination.latitude, Flight.origin_airport_code, Destination.airport_code, Destination.longitude, Flight.departure_date, Flight.flight_id, Flight.return_date FROM Surf
JOIN Destination ON Destination.location_id = Surf.location_id
JOIN Flight ON Destination.location_id = Surf.location_id
WHERE Surf.spot_name = 'Tavarua-Cloudbreak' AND 
Flight.flight_id = 2182 AND log_id IN
(SELECT TOP 80 log_id FROM SURF ORDER BY log_id DESC)
ORDER BY forecast_for_date ASC;

SELECT AVG(Surf.swell_height_feet) AS historical_average_surf, 
AVG(Flight.price) AS historical_average_price, 
MIN(Flight.price) AS historical_min_price, 
MAX(Flight.price) AS historical_max_price
FROM Surf
JOIN Destination ON Destination.location_id = Surf.location_id
JOIN Flight ON Destination.airport_code = Flight.airport_code
WHERE Surf.spot_name = 'Uluwatu'

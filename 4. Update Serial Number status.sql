UPDATE Material 
SET Requires_Serial_Number = 'Yes'
WHERE Material_ID IN (SELECT DISTINCT Material_ID FROM Stock WHERE Serial_Number IS NOT NULL);

UPDATE Material 
SET Requires_Serial_Number = 'No'
WHERE Material_ID IN (SELECT DISTINCT Material_ID FROM Stock WHERE Serial_Number IS NULL);


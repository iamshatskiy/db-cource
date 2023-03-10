
CREATE OR REPLACE FUNCTION delete_advert(ad_id INT)
RETURNS void AS $$
BEGIN
	delete from Deals where AdvertId = ad_id;
	delete from Cat_Photos where Id in (select Cat_PhotoId from Cat_Advert where Id = ad_id);
	delete from Cat_Advert where Id = ad_id;
END;
$$ LANGUAGE PLPGSQL;
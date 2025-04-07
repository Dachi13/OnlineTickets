CREATE TABLE public."Categories"
(
    "Id"   BIGSERIAL NOT NULL,
    "Name" TEXT    NOT NULL,
    PRIMARY KEY ("Id")
);

CREATE TABLE public."Events"
(
    "Id" BIGSERIAL NOT NULL,
    "Name" text NOT NULL,
    "Description" text,
    "Location" text NOT NULL,
    "CategoryId" bigint NOT NULL,
    "AmountOfTickets" integer,
    "StartTime" timestamp without time zone NOT NULL,
    "EndTime" timestamp without time zone NOT NULL,
    "ImageFile" bytea,
    "IsDeleted" boolean NOT NULL DEFAULT false,
    PRIMARY KEY ("Id"),
    FOREIGN KEY ("CategoryId")
        REFERENCES public."Categories" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID
);


CREATE OR REPLACE PROCEDURE public.spAddEvent(
    IN p_name TEXT,
	IN p_description TEXT,
	IN p_location TEXT,
    IN p_categoryid INTEGER,
	IN p_amountOfTickets INTEGER,
	IN p_startTime timestamp without time zone,
	IN p_endTime timestamp without time zone,
    IN p_imagefile BYTEA,
    OUT eventid BIGINT)
LANGUAGE 'plpgsql'
AS $$
BEGIN
INSERT INTO public."Events" (
    "Name",
    "Description",
    "Location",
    "CategoryId",
    "AmountOfTickets",
    "StartTime",
    "EndTime",
    "ImageFile",
    "IsDeleted"
)
VALUES (
           p_name,
           p_description,
           p_location,
           p_categoryid,
           p_amountOfTickets,
           p_startTime,
           p_endTime,
           p_imagefile,
           false
       ) RETURNING "Id" INTO eventid;
END;
$$;

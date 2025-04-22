CREATE TABLE public."Persons"
(
    "Id"   BIGSERIAL NOT NULL,
    "Name" TEXT      NOT NULL,
    "Surname" TEXT      NOT NULL,
    "Email" TEXT      NOT NULL,
    "DoB" timestamp without time zone NOT NULL,
    "IsDeleted" boolean   NOT NULL DEFAULT false,
    PRIMARY KEY ("Id")
);

INSERT INTO public."Persons"(
    "Name", "Surname", "Email", "DoB")
VALUES ('Dachi', 'Lekborashvili', 'Lekborashvili@gmail.com', '"2002-12-13 12:00:00"');
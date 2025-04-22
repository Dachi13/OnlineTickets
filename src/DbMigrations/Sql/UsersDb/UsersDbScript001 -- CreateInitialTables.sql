CREATE TABLE public."Users"
(
    "Id"       BIGSERIAL NOT NULL,
    "Email"    TEXT      NOT NULL,
    "UserName" TEXT      NOT NULL,
    "Password" TEXT      NOT NULL,
    PRIMARY KEY ("Id")
);

CREATE PROCEDURE public.spAddUser(
    IN p_email TEXT,
    IN p_username TEXT,
    IN p_password TEXT,
    OUT userid BIGINT
)
    LANGUAGE 'plpgsql'
AS $$
BEGIN

INSERT INTO public."Users"("Email",
                           "UserName",
                           "Password")
VALUES (p_email,
        p_username,
        p_password) RETURNING "Id"
INTO userid;
END;
$$;
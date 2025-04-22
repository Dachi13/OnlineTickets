CREATE TABLE public."Baskets"
(
    "Id" bigint NOT NULL,
    "TicketId" bigint NOT NULL,
    "Amount" integer NOT NULL DEFAULT 1,
    "TotalPrice" numeric,
    "DiscountId" bigint,
    "IsDeleted" boolean NOT NULL DEFAULT false,
    PRIMARY KEY ("Id", "TicketId")
);


CREATE OR REPLACE PROCEDURE public.spaddbasket(
	IN p_basketid bigint,
	IN p_ticketid bigint,
	IN p_amount integer,
	IN p_price numeric,
	IN p_discountid bigint,
	OUT basketid bigint)
LANGUAGE 'plpgsql'
AS $$
BEGIN
	IF p_basketId IS NULL THEN
SELECT COALESCE(MAX("Id"), 0) + 1 INTO BasketId FROM public."Baskets";
ELSE
        basketid := p_basketId;
END IF;

INSERT INTO public."Baskets"
(
    "Id",
    "TicketId",
    "Amount",
    "TotalPrice",
    "DiscountId",
    "IsDeleted"
)
VALUES
    (
        basketid,
        p_ticketId,
        p_amount,
        p_price,
        p_discountId,
        FALSE
    );
END;
$$;


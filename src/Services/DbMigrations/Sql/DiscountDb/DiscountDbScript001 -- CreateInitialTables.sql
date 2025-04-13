CREATE TABLE public."Discounts"
(
    "Id"          bigserial NOT NULL,
    "CategoryId"  bigint    NOT NULL,
    "Description" text      NOT NULL,
    "Amount"      numeric,
    "IsDeleted"   boolean   NOT NULL DEFAULT false,
    PRIMARY KEY ("DiscountId")
);

CREATE PROCEDURE public.spAddDiscount(
    IN p_categoryId BIGINT,
    IN p_description TEXT,
    IN p_amount NUMERIC,
    OUT discountId BIGINT)
    LANGUAGE 'plpgsql'
        AS $$
BEGIN
INSERT INTO public."Discounts"("CategoryId",
                               "Description",
                               "Amount",
                               "IsDeleted")
VALUES (p_categoryId,
        p_description,
        p_amount,
        FALSE) RETURNING "Id"
INTO discountId;
END;
$$;

--
-- PostgreSQL database dump
--

-- Dumped from database version 13.4
-- Dumped by pg_dump version 13.4

-- Started on 2022-06-02 22:06:03

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 240 (class 1255 OID 467583)
-- Name: delete_advert(integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.delete_advert(ad_id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
	delete from public."Deals" where "AdvertId" = ad_id;
	delete from public."Cat_Adverts" where "Id" = ad_id;
	delete from public."Cat_Photos" where "Id" in (select "Cat_PhotoId" from public."Cat_Adverts" where "Id" = ad_id);
END;
$$;


ALTER FUNCTION public.delete_advert(ad_id integer) OWNER TO postgres;

--
-- TOC entry 241 (class 1255 OID 549607)
-- Name: delete_deal(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.delete_deal() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN	
		if new."IsConfirmedByOWner" = -1 THEN
			delete from public."Deals" where "Id" = new."Id";
			perform insert_for_admin(new."BuyerId", new."OwnerId", false);
		else
			if new."IsConfirmedByBuyer" = 1 THEN
				perform delete_advert(new."AdvertId");
				perform insert_for_admin(new."BuyerId", new."OwnerId", true);
				perform insert_feedbacks(new."BuyerId", new."OwnerId");
			elseif new."IsConfirmedByBuyer" = -1 THEN
				delete from public."Deals" where "Id" = new."Id";
				perform insert_for_admin(new."BuyerId", new."OwnerId", false);
			end if;
			
		end if;
	RETURN NEW;
END;
$$;


ALTER FUNCTION public.delete_deal() OWNER TO postgres;

--
-- TOC entry 243 (class 1255 OID 467667)
-- Name: delete_transac(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.delete_transac() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN	
		if new.isconfimedbyowner = false THEN
			delete from transations where id = new.id;
			perform insert_for_admin(new.buyerid, new.ownerid, false);
		else
			if new.isconfimedbyuser = true THEN
				perform delete_advert(new.catid);
				perform insert_for_admin(new.buyerid, new.ownerid, true);
			elseif new.isconfimedbyuser = false THEN
				delete from transations where id = new.id;
				perform insert_for_admin(new.buyerid, new.ownerid, false);
			end if;
			
		end if;
	RETURN NEW;
END;
$$;


ALTER FUNCTION public.delete_transac() OWNER TO postgres;

--
-- TOC entry 228 (class 1255 OID 549610)
-- Name: insert_feedbacks(text, text); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.insert_feedbacks(buyer_id text, owner_id text) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
	INSERT INTO public."Feedbacks"("BuyerId", "FbDate", "Feedback", "OwnerId", "Rate") values
					(buyer_id, CURRENT_TIMESTAMP, 'default', owner_id, 0);
END;
$$;


ALTER FUNCTION public.insert_feedbacks(buyer_id text, owner_id text) OWNER TO postgres;

--
-- TOC entry 242 (class 1255 OID 467564)
-- Name: insert_for_admin(integer, integer, boolean); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.insert_for_admin(buyer_id integer, owner_id integer, isconf boolean) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
	INSERT INTO transations_history_admin(buyerid, isbuyercur, ownerid, isownercur, dateofclose, isconfimed) values
					(buyer_id, true, owner_id, true, current_date, isconf);
END;
$$;


ALTER FUNCTION public.insert_for_admin(buyer_id integer, owner_id integer, isconf boolean) OWNER TO postgres;

--
-- TOC entry 227 (class 1255 OID 549600)
-- Name: insert_for_admin(text, text, boolean); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.insert_for_admin(buyer_id text, owner_id text, isconf boolean) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
	INSERT INTO public."DealHistory"("BuyerId", "IsConfirmed", "OwnerId") values
					(buyer_id, isconf, owner_id);
END;
$$;


ALTER FUNCTION public.insert_for_admin(buyer_id text, owner_id text, isconf boolean) OWNER TO postgres;

--
-- TOC entry 226 (class 1255 OID 467636)
-- Name: user_delete(integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.user_delete(u_id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
	delete from users where id = u_id;
END;
$$;


ALTER FUNCTION public.user_delete(u_id integer) OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 206 (class 1259 OID 484076)
-- Name: AspNetRoleClaims; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."AspNetRoleClaims" (
    "Id" integer NOT NULL,
    "ClaimType" text,
    "ClaimValue" text,
    "RoleId" text NOT NULL
);


ALTER TABLE public."AspNetRoleClaims" OWNER TO postgres;

--
-- TOC entry 205 (class 1259 OID 484074)
-- Name: AspNetRoleClaims_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."AspNetRoleClaims_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."AspNetRoleClaims_Id_seq" OWNER TO postgres;

--
-- TOC entry 3176 (class 0 OID 0)
-- Dependencies: 205
-- Name: AspNetRoleClaims_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."AspNetRoleClaims_Id_seq" OWNED BY public."AspNetRoleClaims"."Id";


--
-- TOC entry 203 (class 1259 OID 484058)
-- Name: AspNetRoles; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."AspNetRoles" (
    "Id" text NOT NULL,
    "ConcurrencyStamp" text,
    "Name" character varying(256),
    "NormalizedName" character varying(256)
);


ALTER TABLE public."AspNetRoles" OWNER TO postgres;

--
-- TOC entry 208 (class 1259 OID 484092)
-- Name: AspNetUserClaims; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."AspNetUserClaims" (
    "Id" integer NOT NULL,
    "ClaimType" text,
    "ClaimValue" text,
    "UserId" text NOT NULL
);


ALTER TABLE public."AspNetUserClaims" OWNER TO postgres;

--
-- TOC entry 207 (class 1259 OID 484090)
-- Name: AspNetUserClaims_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."AspNetUserClaims_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."AspNetUserClaims_Id_seq" OWNER TO postgres;

--
-- TOC entry 3177 (class 0 OID 0)
-- Dependencies: 207
-- Name: AspNetUserClaims_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."AspNetUserClaims_Id_seq" OWNED BY public."AspNetUserClaims"."Id";


--
-- TOC entry 209 (class 1259 OID 484106)
-- Name: AspNetUserLogins; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."AspNetUserLogins" (
    "LoginProvider" text NOT NULL,
    "ProviderKey" text NOT NULL,
    "ProviderDisplayName" text,
    "UserId" text NOT NULL
);


ALTER TABLE public."AspNetUserLogins" OWNER TO postgres;

--
-- TOC entry 210 (class 1259 OID 484119)
-- Name: AspNetUserRoles; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."AspNetUserRoles" (
    "UserId" text NOT NULL,
    "RoleId" text NOT NULL
);


ALTER TABLE public."AspNetUserRoles" OWNER TO postgres;

--
-- TOC entry 211 (class 1259 OID 484137)
-- Name: AspNetUserTokens; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."AspNetUserTokens" (
    "UserId" text NOT NULL,
    "LoginProvider" text NOT NULL,
    "Name" text NOT NULL,
    "Value" text
);


ALTER TABLE public."AspNetUserTokens" OWNER TO postgres;

--
-- TOC entry 204 (class 1259 OID 484066)
-- Name: AspNetUsers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."AspNetUsers" (
    "Id" text NOT NULL,
    "AccessFailedCount" integer NOT NULL,
    "ConcurrencyStamp" text,
    "Email" character varying(256),
    "EmailConfirmed" boolean NOT NULL,
    "LockoutEnabled" boolean NOT NULL,
    "LockoutEnd" timestamp with time zone,
    "NormalizedEmail" character varying(256),
    "NormalizedUserName" character varying(256),
    "PasswordHash" text,
    "PhoneNumber" text,
    "PhoneNumberConfirmed" boolean NOT NULL,
    "SecurityStamp" text,
    "TwoFactorEnabled" boolean NOT NULL,
    "UserName" character varying(256),
    city text,
    name text,
    surname text
);


ALTER TABLE public."AspNetUsers" OWNER TO postgres;

--
-- TOC entry 213 (class 1259 OID 492247)
-- Name: Breeds; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Breeds" (
    "Id" integer NOT NULL,
    "Breed_name" text
);


ALTER TABLE public."Breeds" OWNER TO postgres;

--
-- TOC entry 212 (class 1259 OID 492245)
-- Name: Breeds_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Breeds_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."Breeds_Id_seq" OWNER TO postgres;

--
-- TOC entry 3178 (class 0 OID 0)
-- Dependencies: 212
-- Name: Breeds_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Breeds_Id_seq" OWNED BY public."Breeds"."Id";


--
-- TOC entry 217 (class 1259 OID 492277)
-- Name: Cat_Adverts; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Cat_Adverts" (
    "Id" integer NOT NULL,
    "Age" integer NOT NULL,
    "ApplicationUserId" text,
    "BreedId" integer NOT NULL,
    "Cat_PhotoId" integer,
    "Description" text,
    "Is_vis" boolean NOT NULL,
    "Price" integer NOT NULL,
    "AdvertDate" timestamp without time zone DEFAULT '0001-01-01 00:00:00'::timestamp without time zone NOT NULL
);


ALTER TABLE public."Cat_Adverts" OWNER TO postgres;

--
-- TOC entry 216 (class 1259 OID 492275)
-- Name: Cat_Adverts_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Cat_Adverts_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."Cat_Adverts_Id_seq" OWNER TO postgres;

--
-- TOC entry 3179 (class 0 OID 0)
-- Dependencies: 216
-- Name: Cat_Adverts_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Cat_Adverts_Id_seq" OWNED BY public."Cat_Adverts"."Id";


--
-- TOC entry 215 (class 1259 OID 492258)
-- Name: Cat_Photos; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Cat_Photos" (
    "Id" integer NOT NULL,
    "Image" text,
    "Name" text
);


ALTER TABLE public."Cat_Photos" OWNER TO postgres;

--
-- TOC entry 214 (class 1259 OID 492256)
-- Name: Cat_Photos_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Cat_Photos_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."Cat_Photos_Id_seq" OWNER TO postgres;

--
-- TOC entry 3180 (class 0 OID 0)
-- Dependencies: 214
-- Name: Cat_Photos_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Cat_Photos_Id_seq" OWNED BY public."Cat_Photos"."Id";


--
-- TOC entry 225 (class 1259 OID 549591)
-- Name: DealHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."DealHistory" (
    "Id" integer NOT NULL,
    "BuyerId" text,
    "IsConfirmed" boolean NOT NULL,
    "OwnerId" text
);


ALTER TABLE public."DealHistory" OWNER TO postgres;

--
-- TOC entry 224 (class 1259 OID 549589)
-- Name: DealHistory_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."DealHistory_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."DealHistory_Id_seq" OWNER TO postgres;

--
-- TOC entry 3181 (class 0 OID 0)
-- Dependencies: 224
-- Name: DealHistory_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."DealHistory_Id_seq" OWNED BY public."DealHistory"."Id";


--
-- TOC entry 219 (class 1259 OID 533207)
-- Name: Deals; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Deals" (
    "Id" integer NOT NULL,
    "AdvertId" integer NOT NULL,
    "BuyerId" text,
    "IsConfirmedByBuyer" integer NOT NULL,
    "IsConfirmedByOWner" integer NOT NULL,
    "OwnerId" text
);


ALTER TABLE public."Deals" OWNER TO postgres;

--
-- TOC entry 218 (class 1259 OID 533205)
-- Name: Deals_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Deals_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."Deals_Id_seq" OWNER TO postgres;

--
-- TOC entry 3182 (class 0 OID 0)
-- Dependencies: 218
-- Name: Deals_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Deals_Id_seq" OWNED BY public."Deals"."Id";


--
-- TOC entry 221 (class 1259 OID 533240)
-- Name: Feedbacks; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Feedbacks" (
    "Id" integer NOT NULL,
    "BuyerId" text,
    "FbDate" timestamp without time zone,
    "Feedback" text,
    "OwnerId" text,
    "Rate" integer NOT NULL
);


ALTER TABLE public."Feedbacks" OWNER TO postgres;

--
-- TOC entry 220 (class 1259 OID 533238)
-- Name: Feedbacks_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Feedbacks_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."Feedbacks_Id_seq" OWNER TO postgres;

--
-- TOC entry 3183 (class 0 OID 0)
-- Dependencies: 220
-- Name: Feedbacks_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Feedbacks_Id_seq" OWNED BY public."Feedbacks"."Id";


--
-- TOC entry 223 (class 1259 OID 541399)
-- Name: HideComments; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."HideComments" (
    "Id" integer NOT NULL,
    "AdvertId" integer NOT NULL,
    "Comment" text
);


ALTER TABLE public."HideComments" OWNER TO postgres;

--
-- TOC entry 222 (class 1259 OID 541397)
-- Name: HideComments_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."HideComments_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."HideComments_Id_seq" OWNER TO postgres;

--
-- TOC entry 3184 (class 0 OID 0)
-- Dependencies: 222
-- Name: HideComments_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."HideComments_Id_seq" OWNED BY public."HideComments"."Id";


--
-- TOC entry 202 (class 1259 OID 484053)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- TOC entry 2945 (class 2604 OID 484079)
-- Name: AspNetRoleClaims Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AspNetRoleClaims" ALTER COLUMN "Id" SET DEFAULT nextval('public."AspNetRoleClaims_Id_seq"'::regclass);


--
-- TOC entry 2946 (class 2604 OID 484095)
-- Name: AspNetUserClaims Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AspNetUserClaims" ALTER COLUMN "Id" SET DEFAULT nextval('public."AspNetUserClaims_Id_seq"'::regclass);


--
-- TOC entry 2947 (class 2604 OID 492250)
-- Name: Breeds Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Breeds" ALTER COLUMN "Id" SET DEFAULT nextval('public."Breeds_Id_seq"'::regclass);


--
-- TOC entry 2949 (class 2604 OID 492280)
-- Name: Cat_Adverts Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Cat_Adverts" ALTER COLUMN "Id" SET DEFAULT nextval('public."Cat_Adverts_Id_seq"'::regclass);


--
-- TOC entry 2948 (class 2604 OID 492261)
-- Name: Cat_Photos Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Cat_Photos" ALTER COLUMN "Id" SET DEFAULT nextval('public."Cat_Photos_Id_seq"'::regclass);


--
-- TOC entry 2954 (class 2604 OID 549594)
-- Name: DealHistory Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."DealHistory" ALTER COLUMN "Id" SET DEFAULT nextval('public."DealHistory_Id_seq"'::regclass);


--
-- TOC entry 2951 (class 2604 OID 533210)
-- Name: Deals Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Deals" ALTER COLUMN "Id" SET DEFAULT nextval('public."Deals_Id_seq"'::regclass);


--
-- TOC entry 2952 (class 2604 OID 533243)
-- Name: Feedbacks Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Feedbacks" ALTER COLUMN "Id" SET DEFAULT nextval('public."Feedbacks_Id_seq"'::regclass);


--
-- TOC entry 2953 (class 2604 OID 541402)
-- Name: HideComments Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."HideComments" ALTER COLUMN "Id" SET DEFAULT nextval('public."HideComments_Id_seq"'::regclass);


--
-- TOC entry 3151 (class 0 OID 484076)
-- Dependencies: 206
-- Data for Name: AspNetRoleClaims; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."AspNetRoleClaims" ("Id", "ClaimType", "ClaimValue", "RoleId") FROM stdin;
\.


--
-- TOC entry 3148 (class 0 OID 484058)
-- Dependencies: 203
-- Data for Name: AspNetRoles; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."AspNetRoles" ("Id", "ConcurrencyStamp", "Name", "NormalizedName") FROM stdin;
467eda3f-1e0b-4ae7-b75e-477c7a388465	1ea54d2f-99ca-4c4e-a55a-40ed11ebea63	moderator	MODERATOR
c21540a4-105b-4c07-b880-c37abc231f7c	6e7d8f2b-e4e6-4074-ab76-f47c7e0663bc	admin	ADMIN
6cc86419-b689-42fd-a48b-8b91f277577a	d7d2de6e-bebc-45db-ac07-71e621825fe3	user	USER
\.


--
-- TOC entry 3153 (class 0 OID 484092)
-- Dependencies: 208
-- Data for Name: AspNetUserClaims; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."AspNetUserClaims" ("Id", "ClaimType", "ClaimValue", "UserId") FROM stdin;
\.


--
-- TOC entry 3154 (class 0 OID 484106)
-- Dependencies: 209
-- Data for Name: AspNetUserLogins; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."AspNetUserLogins" ("LoginProvider", "ProviderKey", "ProviderDisplayName", "UserId") FROM stdin;
\.


--
-- TOC entry 3155 (class 0 OID 484119)
-- Dependencies: 210
-- Data for Name: AspNetUserRoles; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."AspNetUserRoles" ("UserId", "RoleId") FROM stdin;
c4294e7e-4501-48e2-b00c-b4e344ca3f72	c21540a4-105b-4c07-b880-c37abc231f7c
45890fd1-ebb2-4028-94e3-7b51202876e9	467eda3f-1e0b-4ae7-b75e-477c7a388465
\.


--
-- TOC entry 3156 (class 0 OID 484137)
-- Dependencies: 211
-- Data for Name: AspNetUserTokens; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."AspNetUserTokens" ("UserId", "LoginProvider", "Name", "Value") FROM stdin;
452b06f2-d926-429e-a4bb-fd1999cc557b	[AspNetUserStore]	AuthenticatorKey	XC2FYIGBL2XMFWGFZLPI5IAPJTBMKUJH
\.


--
-- TOC entry 3149 (class 0 OID 484066)
-- Dependencies: 204
-- Data for Name: AspNetUsers; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."AspNetUsers" ("Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", city, name, surname) FROM stdin;
452b06f2-d926-429e-a4bb-fd1999cc557b	0	f20af8d6-85ca-4ced-9e04-921a35be2974	nikita.shatskiy@gmail.com	f	t	\N	NIKITA.SHATSKIY@GMAIL.COM	NIKITA.SHATSKIY@GMAIL.COM	AQAAAAEAACcQAAAAEKn5N7Gj7KE1GGUA1ySNMjfA4v1IRwnBO3+r3/0XuJXKcxt1fXaDYlsURBs2TJxUMg==	12345678	f	65e59b9b-6c33-4ae3-8338-5edeb43a184c	f	nikita.shatskiy@gmail.com	Щелково	Никита	Шацкий
c4294e7e-4501-48e2-b00c-b4e344ca3f72	0	1779c065-6bb9-4379-b2a8-f033aea0dd0a	admin@gmail.com	f	t	\N	ADMIN@GMAIL.COM	ADMIN@GMAIL.COM	AQAAAAEAACcQAAAAEJjO2KCYNGABkcTUG115+gv8Ukv2rjA+axlbh9zkH3VVsG23h3qvHljiy/n/Cjgobg==	8904324231	f	a6aec4f4-57a4-4285-b3b1-c09c5f159e45	f	admin@gmail.com	Фрязино	Админ	Шацкий
45890fd1-ebb2-4028-94e3-7b51202876e9	0	27a81ec2-436c-49e5-9b8f-f5e7202f17c7	moderator@m.com	f	t	\N	MODERATOR@M.COM	MODERATOR@M.COM	AQAAAAEAACcQAAAAEAgsH7dP3G4ry5mnPeEyhOXQ6DxgA4XoLZ5C6YDinn0C2XQ4egqAqlS+KyXyTd91Fw==	\N	f	4c3b21bf-fddd-4bfb-b6f3-747d98131f97	f	moderator@m.com	\N	Модератор	\N
\.


--
-- TOC entry 3158 (class 0 OID 492247)
-- Dependencies: 213
-- Data for Name: Breeds; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Breeds" ("Id", "Breed_name") FROM stdin;
1	Абиссинская
2	Американская Жесткошерстная
3	Азиатская
4	Австралийский Мист
5	Балийская
6	Турецкий Ван
7	Тонкинская
8	Шантильи-тиффани
9	Сомайлиская
10	Сфинкс
11	Донской Сфинкс
12	Сноу-Шу
13	Сингапура
14	Селкирс-Рекс
15	Шотландская Вислоухая
16	Саванна
17	Русская голубая
18	Рэгдолл
19	Пиксибоб
20	Персидская Длинношерстная
21	Восточная короткошерстная
22	Восточная Длинношерстная
23	Оцикет
24	Норвежская лесная
25	Манчкин
26	Мэнкс
27	Лаперм
28	Корат
29	Као-Мани
30	Японский Бобтейл Короткошерстный
31	Японский Бобтейл Длинношерстный
32	Экзотическая Короткошерстная
33	Египетский Мау
34	Девон-Рекс
35	Кимрик
36	Корниш-Рекс
37	Шиншилла
38	Бурмилла
39	Бурманская Кошка
40	Британская Короткошерстная
41	Бирманская
42	Рагамаффин
43	Тойгер
44	Ликой
45	Нибелунг
46	Неизвестно
\.


--
-- TOC entry 3162 (class 0 OID 492277)
-- Dependencies: 217
-- Data for Name: Cat_Adverts; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Cat_Adverts" ("Id", "Age", "ApplicationUserId", "BreedId", "Cat_PhotoId", "Description", "Is_vis", "Price", "AdvertDate") FROM stdin;
16	6	c4294e7e-4501-48e2-b00c-b4e344ca3f72	40	24	Гендальф, балдежник	t	150000	2022-05-22 16:09:25.034993
13	10	c4294e7e-4501-48e2-b00c-b4e344ca3f72	41	16	Уверенный в себе кот, покупайте	t	15000	2022-05-15 21:55:39.619848
18	4	452b06f2-d926-429e-a4bb-fd1999cc557b	16	26	Хороший кот	t	0	2022-05-22 21:31:37.048784
\.


--
-- TOC entry 3160 (class 0 OID 492258)
-- Dependencies: 215
-- Data for Name: Cat_Photos; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Cat_Photos" ("Id", "Image", "Name") FROM stdin;
13	C:\\Users\\nikit\\source\\repos\\купикота.рф\\купикота.рф\\wwwroot\\images\\7f16c3c4-8226-4235-b6b6-cb0116acf482_set.png	7f16c3c4-8226-4235-b6b6-cb0116acf482_set.png
14	\N	\N
15	\N	\N
16	C:\\Users\\nikit\\source\\repos\\купикота.рф\\купикота.рф\\wwwroot\\images\\cf773f5e-1427-4d52-90f3-2e28af3c6fe2_180129dfe537d9cec25e1f884286eb79.jpg	cf773f5e-1427-4d52-90f3-2e28af3c6fe2_180129dfe537d9cec25e1f884286eb79.jpg
17	C:\\Users\\nikit\\source\\repos\\купикота.рф\\купикота.рф\\wwwroot\\images\\6527e756-98fc-4f49-a669-7e1bcb665f04_cat1503.jpg	6527e756-98fc-4f49-a669-7e1bcb665f04_cat1503.jpg
18	C:\\Users\\nikit\\source\\repos\\купикота.рф\\купикота.рф\\wwwroot\\images\\010b318a-8da9-484a-9891-e46741b3198a_Gendalf.png	010b318a-8da9-484a-9891-e46741b3198a_Gendalf.png
19	\N	\N
20	\N	\N
21	\N	\N
22	C:\\Users\\nikit\\source\\repos\\купикота.рф\\купикота.рф\\wwwroot\\images\\8d774330-5085-4ab1-89e9-f070d7348208_000029_1624895113_448053_big.jpg	8d774330-5085-4ab1-89e9-f070d7348208_000029_1624895113_448053_big.jpg
23	C:\\Users\\nikit\\source\\repos\\купикота.рф\\купикота.рф\\wwwroot\\images\\41397e89-c3a2-40cc-90f1-48803246c13e_1-6.jpg	41397e89-c3a2-40cc-90f1-48803246c13e_1-6.jpg
24	C:\\Users\\nikit\\source\\repos\\купикота.рф\\купикота.рф\\wwwroot\\images\\e0dca1ac-a52e-4ab6-9e5e-e3f88a217e01_Gendalf.png	e0dca1ac-a52e-4ab6-9e5e-e3f88a217e01_Gendalf.png
25	C:\\Users\\nikit\\source\\repos\\купикота.рф\\купикота.рф\\wwwroot\\images\\20b87c45-62fc-4cad-bea3-01b9dface2c0_main.animal.cats.jpg	20b87c45-62fc-4cad-bea3-01b9dface2c0_main.animal.cats.jpg
26	C:\\Users\\nikit\\source\\repos\\купикота.рф\\купикота.рф\\wwwroot\\images\\0b40c084-8ff7-4477-b7e9-36ecb6183b0c_main.animal.cats.jpg	0b40c084-8ff7-4477-b7e9-36ecb6183b0c_main.animal.cats.jpg
\.


--
-- TOC entry 3170 (class 0 OID 549591)
-- Dependencies: 225
-- Data for Name: DealHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."DealHistory" ("Id", "BuyerId", "IsConfirmed", "OwnerId") FROM stdin;
1	c4294e7e-4501-48e2-b00c-b4e344ca3f72	t	452b06f2-d926-429e-a4bb-fd1999cc557b
2	452b06f2-d926-429e-a4bb-fd1999cc557b	t	c4294e7e-4501-48e2-b00c-b4e344ca3f72
3	c4294e7e-4501-48e2-b00c-b4e344ca3f72	f	452b06f2-d926-429e-a4bb-fd1999cc557b
4	452b06f2-d926-429e-a4bb-fd1999cc557b	t	c4294e7e-4501-48e2-b00c-b4e344ca3f72
5	452b06f2-d926-429e-a4bb-fd1999cc557b	f	c4294e7e-4501-48e2-b00c-b4e344ca3f72
10	c4294e7e-4501-48e2-b00c-b4e344ca3f72	t	452b06f2-d926-429e-a4bb-fd1999cc557b
11	452b06f2-d926-429e-a4bb-fd1999cc557b	t	c4294e7e-4501-48e2-b00c-b4e344ca3f72
13	c4294e7e-4501-48e2-b00c-b4e344ca3f72	t	452b06f2-d926-429e-a4bb-fd1999cc557b
\.


--
-- TOC entry 3164 (class 0 OID 533207)
-- Dependencies: 219
-- Data for Name: Deals; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Deals" ("Id", "AdvertId", "BuyerId", "IsConfirmedByBuyer", "IsConfirmedByOWner", "OwnerId") FROM stdin;
\.


--
-- TOC entry 3166 (class 0 OID 533240)
-- Dependencies: 221
-- Data for Name: Feedbacks; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Feedbacks" ("Id", "BuyerId", "FbDate", "Feedback", "OwnerId", "Rate") FROM stdin;
\.


--
-- TOC entry 3168 (class 0 OID 541399)
-- Dependencies: 223
-- Data for Name: HideComments; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."HideComments" ("Id", "AdvertId", "Comment") FROM stdin;
2	18	Замечание
3	18	Укажите более подробное описание.\r\n
\.


--
-- TOC entry 3147 (class 0 OID 484053)
-- Dependencies: 202
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20220411211451_f	2.0.3-rtm-10026
20220422191440_AdvertTables	2.0.3-rtm-10026
20220422195853_AdvertTablesq	2.0.3-rtm-10026
20220422200239_AdvertTables2	2.0.3-rtm-10026
20220422200720_AdvertTables3	2.0.3-rtm-10026
20220423072901_AdvertTables4	2.0.3-rtm-10026
20220423073102_AdvertTables5	2.0.3-rtm-10026
20220423073204_AdvertTables6	2.0.3-rtm-10026
20220423074431_AdvertTables7	2.0.3-rtm-10026
20220423075036_AdvertTables7	2.0.3-rtm-10026
20220423075243_AdvertTables7	2.0.3-rtm-10026
20220424212245_Date	2.0.3-rtm-10026
20220508143646_asds	2.0.3-rtm-10026
20220514210314_new_t	2.0.3-rtm-10026
20220519201621_DEALS	2.0.3-rtm-10026
20220519201755_DEALS fix	2.0.3-rtm-10026
20220519202838_Feedbacks	2.0.3-rtm-10026
20220521130907_HideCom	2.0.3-rtm-10026
20220522110638_DealHistory	2.0.3-rtm-10026
20220530001311_sbyte_to_int	2.0.3-rtm-10026
\.


--
-- TOC entry 3185 (class 0 OID 0)
-- Dependencies: 205
-- Name: AspNetRoleClaims_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."AspNetRoleClaims_Id_seq"', 1, false);


--
-- TOC entry 3186 (class 0 OID 0)
-- Dependencies: 207
-- Name: AspNetUserClaims_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."AspNetUserClaims_Id_seq"', 1, false);


--
-- TOC entry 3187 (class 0 OID 0)
-- Dependencies: 212
-- Name: Breeds_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Breeds_Id_seq"', 1, false);


--
-- TOC entry 3188 (class 0 OID 0)
-- Dependencies: 216
-- Name: Cat_Adverts_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Cat_Adverts_Id_seq"', 18, true);


--
-- TOC entry 3189 (class 0 OID 0)
-- Dependencies: 214
-- Name: Cat_Photos_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Cat_Photos_Id_seq"', 26, true);


--
-- TOC entry 3190 (class 0 OID 0)
-- Dependencies: 224
-- Name: DealHistory_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."DealHistory_Id_seq"', 13, true);


--
-- TOC entry 3191 (class 0 OID 0)
-- Dependencies: 218
-- Name: Deals_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Deals_Id_seq"', 10, true);


--
-- TOC entry 3192 (class 0 OID 0)
-- Dependencies: 220
-- Name: Feedbacks_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Feedbacks_Id_seq"', 5, true);


--
-- TOC entry 3193 (class 0 OID 0)
-- Dependencies: 222
-- Name: HideComments_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."HideComments_Id_seq"', 3, true);


--
-- TOC entry 2966 (class 2606 OID 484084)
-- Name: AspNetRoleClaims PK_AspNetRoleClaims; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AspNetRoleClaims"
    ADD CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY ("Id");


--
-- TOC entry 2958 (class 2606 OID 484065)
-- Name: AspNetRoles PK_AspNetRoles; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AspNetRoles"
    ADD CONSTRAINT "PK_AspNetRoles" PRIMARY KEY ("Id");


--
-- TOC entry 2969 (class 2606 OID 484100)
-- Name: AspNetUserClaims PK_AspNetUserClaims; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AspNetUserClaims"
    ADD CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY ("Id");


--
-- TOC entry 2972 (class 2606 OID 484113)
-- Name: AspNetUserLogins PK_AspNetUserLogins; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AspNetUserLogins"
    ADD CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey");


--
-- TOC entry 2975 (class 2606 OID 484126)
-- Name: AspNetUserRoles PK_AspNetUserRoles; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AspNetUserRoles"
    ADD CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId");


--
-- TOC entry 2977 (class 2606 OID 484144)
-- Name: AspNetUserTokens PK_AspNetUserTokens; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AspNetUserTokens"
    ADD CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name");


--
-- TOC entry 2962 (class 2606 OID 484073)
-- Name: AspNetUsers PK_AspNetUsers; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AspNetUsers"
    ADD CONSTRAINT "PK_AspNetUsers" PRIMARY KEY ("Id");


--
-- TOC entry 2979 (class 2606 OID 492255)
-- Name: Breeds PK_Breeds; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Breeds"
    ADD CONSTRAINT "PK_Breeds" PRIMARY KEY ("Id");


--
-- TOC entry 2986 (class 2606 OID 492285)
-- Name: Cat_Adverts PK_Cat_Adverts; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Cat_Adverts"
    ADD CONSTRAINT "PK_Cat_Adverts" PRIMARY KEY ("Id");


--
-- TOC entry 2981 (class 2606 OID 492266)
-- Name: Cat_Photos PK_Cat_Photos; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Cat_Photos"
    ADD CONSTRAINT "PK_Cat_Photos" PRIMARY KEY ("Id");


--
-- TOC entry 3000 (class 2606 OID 549599)
-- Name: DealHistory PK_DealHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."DealHistory"
    ADD CONSTRAINT "PK_DealHistory" PRIMARY KEY ("Id");


--
-- TOC entry 2991 (class 2606 OID 533215)
-- Name: Deals PK_Deals; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Deals"
    ADD CONSTRAINT "PK_Deals" PRIMARY KEY ("Id");


--
-- TOC entry 2995 (class 2606 OID 533248)
-- Name: Feedbacks PK_Feedbacks; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Feedbacks"
    ADD CONSTRAINT "PK_Feedbacks" PRIMARY KEY ("Id");


--
-- TOC entry 2998 (class 2606 OID 541407)
-- Name: HideComments PK_HideComments; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."HideComments"
    ADD CONSTRAINT "PK_HideComments" PRIMARY KEY ("Id");


--
-- TOC entry 2956 (class 2606 OID 484057)
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- TOC entry 2960 (class 1259 OID 484155)
-- Name: EmailIndex; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "EmailIndex" ON public."AspNetUsers" USING btree ("NormalizedEmail");


--
-- TOC entry 2964 (class 1259 OID 484150)
-- Name: IX_AspNetRoleClaims_RoleId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON public."AspNetRoleClaims" USING btree ("RoleId");


--
-- TOC entry 2967 (class 1259 OID 484152)
-- Name: IX_AspNetUserClaims_UserId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_AspNetUserClaims_UserId" ON public."AspNetUserClaims" USING btree ("UserId");


--
-- TOC entry 2970 (class 1259 OID 484153)
-- Name: IX_AspNetUserLogins_UserId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_AspNetUserLogins_UserId" ON public."AspNetUserLogins" USING btree ("UserId");


--
-- TOC entry 2973 (class 1259 OID 484154)
-- Name: IX_AspNetUserRoles_RoleId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_AspNetUserRoles_RoleId" ON public."AspNetUserRoles" USING btree ("RoleId");


--
-- TOC entry 2982 (class 1259 OID 492301)
-- Name: IX_Cat_Adverts_ApplicationUserId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Cat_Adverts_ApplicationUserId" ON public."Cat_Adverts" USING btree ("ApplicationUserId");


--
-- TOC entry 2983 (class 1259 OID 492306)
-- Name: IX_Cat_Adverts_BreedId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Cat_Adverts_BreedId" ON public."Cat_Adverts" USING btree ("BreedId");


--
-- TOC entry 2984 (class 1259 OID 492303)
-- Name: IX_Cat_Adverts_Cat_PhotoId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Cat_Adverts_Cat_PhotoId" ON public."Cat_Adverts" USING btree ("Cat_PhotoId");


--
-- TOC entry 2987 (class 1259 OID 533231)
-- Name: IX_Deals_AdvertId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Deals_AdvertId" ON public."Deals" USING btree ("AdvertId");


--
-- TOC entry 2988 (class 1259 OID 533232)
-- Name: IX_Deals_BuyerId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Deals_BuyerId" ON public."Deals" USING btree ("BuyerId");


--
-- TOC entry 2989 (class 1259 OID 533233)
-- Name: IX_Deals_OwnerId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Deals_OwnerId" ON public."Deals" USING btree ("OwnerId");


--
-- TOC entry 2992 (class 1259 OID 533259)
-- Name: IX_Feedbacks_BuyerId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Feedbacks_BuyerId" ON public."Feedbacks" USING btree ("BuyerId");


--
-- TOC entry 2993 (class 1259 OID 533260)
-- Name: IX_Feedbacks_OwnerId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Feedbacks_OwnerId" ON public."Feedbacks" USING btree ("OwnerId");


--
-- TOC entry 2996 (class 1259 OID 541413)
-- Name: IX_HideComments_AdvertId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_HideComments_AdvertId" ON public."HideComments" USING btree ("AdvertId");


--
-- TOC entry 2959 (class 1259 OID 484151)
-- Name: RoleNameIndex; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "RoleNameIndex" ON public."AspNetRoles" USING btree ("NormalizedName");


--
-- TOC entry 2963 (class 1259 OID 484156)
-- Name: UserNameIndex; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "UserNameIndex" ON public."AspNetUsers" USING btree ("NormalizedUserName");


--
-- TOC entry 3016 (class 2620 OID 549608)
-- Name: Deals DealClose; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER "DealClose" AFTER UPDATE ON public."Deals" FOR EACH ROW EXECUTE FUNCTION public.delete_deal();


--
-- TOC entry 3001 (class 2606 OID 484085)
-- Name: AspNetRoleClaims FK_AspNetRoleClaims_AspNetRoles_RoleId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AspNetRoleClaims"
    ADD CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES public."AspNetRoles"("Id") ON DELETE CASCADE;


--
-- TOC entry 3002 (class 2606 OID 484101)
-- Name: AspNetUserClaims FK_AspNetUserClaims_AspNetUsers_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AspNetUserClaims"
    ADD CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE;


--
-- TOC entry 3003 (class 2606 OID 484114)
-- Name: AspNetUserLogins FK_AspNetUserLogins_AspNetUsers_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AspNetUserLogins"
    ADD CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE;


--
-- TOC entry 3004 (class 2606 OID 484127)
-- Name: AspNetUserRoles FK_AspNetUserRoles_AspNetRoles_RoleId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AspNetUserRoles"
    ADD CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES public."AspNetRoles"("Id") ON DELETE CASCADE;


--
-- TOC entry 3005 (class 2606 OID 484132)
-- Name: AspNetUserRoles FK_AspNetUserRoles_AspNetUsers_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AspNetUserRoles"
    ADD CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE;


--
-- TOC entry 3006 (class 2606 OID 484145)
-- Name: AspNetUserTokens FK_AspNetUserTokens_AspNetUsers_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AspNetUserTokens"
    ADD CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE;


--
-- TOC entry 3007 (class 2606 OID 492286)
-- Name: Cat_Adverts FK_Cat_Adverts_AspNetUsers_ApplicationUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Cat_Adverts"
    ADD CONSTRAINT "FK_Cat_Adverts_AspNetUsers_ApplicationUserId" FOREIGN KEY ("ApplicationUserId") REFERENCES public."AspNetUsers"("Id") ON DELETE RESTRICT;


--
-- TOC entry 3009 (class 2606 OID 492307)
-- Name: Cat_Adverts FK_Cat_Adverts_Breeds_BreedId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Cat_Adverts"
    ADD CONSTRAINT "FK_Cat_Adverts_Breeds_BreedId" FOREIGN KEY ("BreedId") REFERENCES public."Breeds"("Id") ON DELETE CASCADE;


--
-- TOC entry 3008 (class 2606 OID 492296)
-- Name: Cat_Adverts FK_Cat_Adverts_Cat_Photos_Cat_PhotoId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Cat_Adverts"
    ADD CONSTRAINT "FK_Cat_Adverts_Cat_Photos_Cat_PhotoId" FOREIGN KEY ("Cat_PhotoId") REFERENCES public."Cat_Photos"("Id") ON DELETE RESTRICT;


--
-- TOC entry 3011 (class 2606 OID 533221)
-- Name: Deals FK_Deals_AspNetUsers_BuyerId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Deals"
    ADD CONSTRAINT "FK_Deals_AspNetUsers_BuyerId" FOREIGN KEY ("BuyerId") REFERENCES public."AspNetUsers"("Id") ON DELETE RESTRICT;


--
-- TOC entry 3012 (class 2606 OID 533226)
-- Name: Deals FK_Deals_AspNetUsers_OwnerId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Deals"
    ADD CONSTRAINT "FK_Deals_AspNetUsers_OwnerId" FOREIGN KEY ("OwnerId") REFERENCES public."AspNetUsers"("Id") ON DELETE RESTRICT;


--
-- TOC entry 3010 (class 2606 OID 533216)
-- Name: Deals FK_Deals_Cat_Adverts_AdvertId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Deals"
    ADD CONSTRAINT "FK_Deals_Cat_Adverts_AdvertId" FOREIGN KEY ("AdvertId") REFERENCES public."Cat_Adverts"("Id") ON DELETE CASCADE;


--
-- TOC entry 3013 (class 2606 OID 533249)
-- Name: Feedbacks FK_Feedbacks_AspNetUsers_BuyerId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Feedbacks"
    ADD CONSTRAINT "FK_Feedbacks_AspNetUsers_BuyerId" FOREIGN KEY ("BuyerId") REFERENCES public."AspNetUsers"("Id") ON DELETE RESTRICT;


--
-- TOC entry 3014 (class 2606 OID 533254)
-- Name: Feedbacks FK_Feedbacks_AspNetUsers_OwnerId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Feedbacks"
    ADD CONSTRAINT "FK_Feedbacks_AspNetUsers_OwnerId" FOREIGN KEY ("OwnerId") REFERENCES public."AspNetUsers"("Id") ON DELETE RESTRICT;


--
-- TOC entry 3015 (class 2606 OID 541408)
-- Name: HideComments FK_HideComments_Cat_Adverts_AdvertId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."HideComments"
    ADD CONSTRAINT "FK_HideComments_Cat_Adverts_AdvertId" FOREIGN KEY ("AdvertId") REFERENCES public."Cat_Adverts"("Id") ON DELETE CASCADE;


-- Completed on 2022-06-02 22:06:04

--
-- PostgreSQL database dump complete
--


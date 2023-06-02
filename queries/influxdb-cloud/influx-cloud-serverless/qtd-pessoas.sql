select count(distinct("personId"))
from (select "value", "personId" from "temperature" WHERE time >= now() - interval '30 minutes')
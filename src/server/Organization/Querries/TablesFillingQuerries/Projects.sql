insert into projects(organization_id, name) 
select id, 'SuperPuperProject' 
from organizations
where name = 'Apple';

insert into projects(organization_id, name)
select id, 'KastelProject'
from organizations
where name = 'Yandex';

insert into projects(organization_id, name)
select id, 'EmigrateProject'
from organizations
where name = 'Yandex';

insert into projects(organization_id, name)
select id, 'YandexTaxi'
from organizations
where name = 'Yandex';
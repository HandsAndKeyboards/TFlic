insert into boards(project_id, name)
select id, 'Developer'
from projects
where name = 'KastelProject';

insert into boards(project_id, name)
select id, 'Manager'
from projects
where name = 'KastelProject';

insert into boards(project_id, name)
select id, 'Supporter'
from projects
where name = 'KastelProject';

insert into boards(project_id, name)
select id, 'Doter'
from projects
where name = 'KastelProject';

insert into boards(project_id, name)
select id, 'Developer'
from projects
where name = 'EmigrateProject';

insert into boards(project_id, name)
select id, 'Manager'
from projects
where name = 'EmigrateProject';

insert into boards(project_id, name)
select id, 'Developer'
from projects
where name = 'SuperPuperProject';

insert into boards(project_id, name)
select id, 'Manager'
from projects
where name = 'SuperPuperProject';

insert into boards(project_id, name)
select id, 'Appler'
from projects
where name = 'SuperPuperProject';
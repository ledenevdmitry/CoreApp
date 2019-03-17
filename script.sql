create tablespace sync
  DATAFILE 'D:\oradata\sync.dbf' 
    SIZE 10M
    AUTOEXTEND ON NEXT 10M MAXSIZE 200M;
	
create user sync
identified by sync
default tablespace sync;

grant create session to sync;

drop table sync.zpatch_hdim;
create table sync.zpatch_hdim (       
zpatch_id NUMBER(20),
parent_id NUMBER(20),
cpatch_id NUMBER(20),
zpatch_name VARCHAR2(200),
validfrom DATE ,
validto DATE,
dwsact VARCHAR2(1),
zpatchstatus VARCHAR2(50))
TABLESPACE sync;

drop table sync.cpatch_hdim;
create table sync.cpatch_hdim  (       
cpatch_id NUMBER(20),
parent_id NUMBER(20),
release_id NUMBER(20),
cpatch_name VARCHAR2(200),
validfrom DATE ,
validto DATE,
dwsact VARCHAR2(1),
cpatchstatus VARCHAR2(50),
kod_sredy VARCHAR2(50))
TABLESPACE sync;

drop table sync.release_hdim;
create table sync.release_hdim  (       
release_id NUMBER(20),
release_name VARCHAR2(200),
validfrom DATE ,
validto DATE,
dwsact VARCHAR2(1))
TABLESPACE sync;

drop table sync.zpatchorder_hdim;
create table sync.zpatchorder_hdim  (       
zpatch_id NUMBER(20),
zpatch_order NUMBER(20),
validfrom DATE ,
validto DATE,
dwsact VARCHAR2(1))
TABLESPACE sync;

create sequence sync.release_seq increment by 1;
create sequence sync.cpatch_seq increment by 1;
create sequence sync.zpatch_seq increment by 1;
alter user sync quota unlimited on sync;

create table sync.cpatchstate (
kod_sredy VARCHAR2(50),
cpatchstatus VARCHAR2(50),
vsspath VARCHAR2(500));

create table sync.zpatchstate (
zpatchstatus VARCHAR2(50),
vsspath VARCHAR2(500));

grant select on sync.cpatchstate to sync;
grant select on sync.zpatchstate to sync;

insert into sync.cpatchstate (kod_sredy, cpatchstatus, vsspath) values ('UNDEFINED', 'UNDEFINED', '$/Patches/Working');
insert into sync.cpatchstate (kod_sredy, cpatchstatus, vsspath) values ('UNDEFINED', 'NOTREADY', '$/Patches/Test/Working');
insert into sync.cpatchstate (kod_sredy, cpatchstatus, vsspath) values ('UNDEFINED', 'READY', '$/Patches/Test/Working STAB');
insert into sync.cpatchstate (kod_sredy, cpatchstatus, vsspath) values ('UNDEFINED', 'INSTALLED', '$/Patches/Working STAB');
insert into sync.cpatchstate (kod_sredy, cpatchstatus, vsspath) values ('UNDEFINED', 'REVISION', '$/Patches/Test/Working');

insert into sync.cpatchstate (kod_sredy, cpatchstatus, vsspath) values ('STAB', 'UNDEFINED', '$/Patches/Working STAB');
insert into sync.cpatchstate (kod_sredy, cpatchstatus, vsspath) values ('STAB', 'NOTREADY', '$/Patches/Test/Working');
insert into sync.cpatchstate (kod_sredy, cpatchstatus, vsspath) values ('STAB', 'READY', '$/Patches/Test/Working STAB');
insert into sync.cpatchstate (kod_sredy, cpatchstatus, vsspath) values ('STAB', 'INSTALLED', '$/Patches/Working STAB');
insert into sync.cpatchstate (kod_sredy, cpatchstatus, vsspath) values ('STAB', 'REVISION', '$/Patches/Test/Working');

insert into sync.cpatchstate (kod_sredy, cpatchstatus, vsspath) values ('TEST', 'UNDEFINED', '$/Patches/VSS_TEST');
insert into sync.cpatchstate (kod_sredy, cpatchstatus, vsspath) values ('TEST', 'NOTREADY', '$/Patches/VSS_TEST');
insert into sync.cpatchstate (kod_sredy, cpatchstatus, vsspath) values ('TEST', 'READY', '$/Patches/VSS_TEST');
insert into sync.cpatchstate (kod_sredy, cpatchstatus, vsspath) values ('TEST', 'INSTALLED', '$/Patches/VSS_TEST');
insert into sync.cpatchstate (kod_sredy, cpatchstatus, vsspath) values ('TEST', 'REVISION', '$/Patches/Test/Working');

insert into sync.zpatchstate (zpatchstatus, vsspath) values ('UNDEFINED', '$/Patches/Test/Working');
insert into sync.zpatchstate (zpatchstatus, vsspath) values ('OPEN', null);
insert into sync.zpatchstate (zpatchstatus, vsspath) values ('READY', '$/Patches/Test/Working');
insert into sync.zpatchstate (zpatchstatus, vsspath) values ('INSTALLED', '$/Patches/Test/Working STAB');
insert into sync.zpatchstate (zpatchstatus, vsspath) values ('ERROR', '$/Patches/Test/Working');



commit;
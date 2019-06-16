create tablespace sync
  DATAFILE 'D:\oradata\sync.dbf' 
    SIZE 10M
    AUTOEXTEND ON NEXT 10M MAXSIZE 200M;
	
create user sync
identified by sync
default tablespace sync;

grant create session to sync;

drop table sync.zpatch;
create table sync.zpatch (       
zpatch_id NUMBER(20),
cpatch_id NUMBER(20),
zpatch_name VARCHAR2(200),
validfrom DATE,
validto DATE,
dwsact VARCHAR2(1),
CR NUMBER(20),
DF NUMBER(20),
Entity VARCHAR2(200),
FSD VARCHAR2(200),
zpatchstatus VARCHAR2(50))
TABLESPACE sync;

alter table sync.zpatch add constraint zpatch_pk1 primary key (zpatch_id, validfrom);

alter table sync.zpatch add constraint zpatch_pk2 unique (zpatch_name);

drop table sync.cpatch;
create table sync.cpatch  (       
cpatch_id NUMBER(20),
release_id NUMBER(20),
cpatch_name VARCHAR2(200),
validfrom DATE,
validto DATE,
dwsact VARCHAR2(1),
cpatchstatus VARCHAR2(50))
TABLESPACE sync;

alter table sync.cpatch add constraint cpatch_pk1 primary key (cpatch_id, validfrom);

alter table sync.cpatch add constraint cpatch_pk2 unique (cpatch_name);

drop table sync.release;
create table sync.release (       
release_id NUMBER(20) ,
release_name VARCHAR2(200),
validfrom DATE ,
validto DATE,
dwsact VARCHAR2(1))
TABLESPACE sync;

alter table sync.release add constraint release_pk1 primary key (release_id, validfrom);

alter table sync.release add constraint release_pk2 unique (release_name);

drop table sync.zpatchorder;
create table sync.zpatchorder  (    
zpatch_order_id NUMBER(20) ,   
zpatch_id NUMBER(20),
zpatch_order NUMBER(20),
validfrom DATE ,
validto DATE,
dwsact VARCHAR2(1))
TABLESPACE sync;

alter table sync.zpatchorder add constraint zpatchorder_pk1 primary key (zpatch_order_id, validfrom);

alter table sync.zpatchorder add constraint zpatchorder_pk2 unique (zpatch_id);

create sequence sync.release_seq increment by 1;
create sequence sync.cpatch_seq increment by 1;
create sequence sync.zpatch_seq increment by 1;
create sequence sync.zpatch_order_seq increment by 1;
alter user sync quota unlimited on sync;

drop table sync.cpatchstate2vsspath;
create table sync.cpatchstate2vsspath (
kod_sredy VARCHAR2(50),
cpatchstatus VARCHAR2(50),
vsspath_id NUMBER(20));

alter table sync.cpatchstate2vsspath add constraint cpatchstate2vsspath_pk primary key (kod_sredy, cpatchstatus);

drop table sync.zpatchstate2vsspath;
create table sync.zpatchstate2vsspath (
kod_sredy VARCHAR2(50) ,
zpatchstatus VARCHAR2(50) ,
vsspath_id NUMBER(20));

alter table sync.zpatchstate2vsspath add constraint zpatchstate2vsspath_pk primary key (kod_sredy, zpatchstatus);

drop table sync.vsspath;
create table sync.vsspath (
vsspath_id NUMBER(20) ,
vsspath VARCHAR2(200));

drop table sync.zpatch_rel;
create table sync.zpatch_rel (
zpatch_rel_id NUMBER(20) ,
zpatch_id_from NUMBER(20),
zpatch_id_to NUMBER(20),
validfrom DATE ,
validto DATE,
dwsact VARCHAR2(1));

alter table sync.zpatch_rel add constraint zpatch_rel_pk1 primary key (zpatch_rel_id, validfrom);

drop table sync.cpatch_rel;
create table sync.cpatch_rel (
cpatch_rel_id NUMBER(20) ,
cpatch_id_from NUMBER(20),
cpatch_id_to NUMBER(20),
validfrom DATE ,
validto DATE,
dwsact VARCHAR2(1));

alter table sync.cpatch_rel add constraint cpatch_rel_pk1 primary key (cpatch_rel_id, validfrom);

commit;
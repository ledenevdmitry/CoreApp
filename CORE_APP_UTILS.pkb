CREATE OR REPLACE PACKAGE BODY SYNC.core_app_utils as
  procedure p_update(i_owner varchar2, i_table_name varchar2, i_id_name varchar2, i_id_value number, i_updating_columns sync.string_table, i_updating_values sync.string_table) is
  l_command varchar2(32000);
  l_insert varchar2(32000);
  l_select varchar2(32000);
  l_updating_columns sync.string_table := i_updating_columns;
  l_updating_values sync.string_table := i_updating_values;
  begin
  
    l_command := 'update ' || i_owner || '.' ||i_table_name || CHR(10) ||
                 'set validto = sysdate ' || CHR(10) || 
                 'where ' || i_id_name || '=' || i_id_value || CHR(10) ||
                 'and dwsact <> ''D''';    
    
    dbms_output.put_line(l_command);    
    execute immediate l_command;
    
    l_insert := 'insert into ' || i_owner || '.' ||i_table_name || '(' || CHR(10);
    l_select := 'select' || CHR(10);            
    
    for i in (
        select column_name 
        from all_tab_columns 
        where owner = i_owner and 
              table_name = i_table_name
        minus
        select * from table(l_updating_columns)
        
        minus
        select * from sync.tech_field_names)
        
        
    loop
        l_insert := l_insert || i.column_name || ',';
        l_select := l_select || i.column_name || ',';
    end loop;
    
    for i in i_updating_columns.first..i_updating_columns.last
    loop
        l_insert := l_insert || l_updating_columns(i) || ',';
        l_select := l_select || l_updating_values(i) || ' ' ||  l_updating_columns(i) || ',';
    end loop;
    
    l_insert := l_insert || 'validfrom, dwsact, validto) ';
    l_select := l_select || 'validto validfrom, ''U'' dwsact, to_date(''59991231'', ''yyyymmdd'') validto ' || CHR(10) ||
                 'from ' || i_owner || '.' ||i_table_name || CHR(10) ||
                 'where '|| i_id_name || '=' || i_id_value || ' and dwsact <> ''D''';
                 
    l_command :=  l_insert || l_select;
    dbms_output.put_line(l_command);
    execute immediate l_command;
    commit;  

end;


procedure p_delete(i_owner varchar2, i_table_name varchar2, i_id_name varchar2, i_id_value number) is
  l_command varchar2(32000);
  l_insert varchar2(32000);
  l_select varchar2(32000);
  begin
  
    l_command := 'update ' || i_owner || '.' ||i_table_name || CHR(10) ||
                 'set validto = sysdate ' || CHR(10) || 
                 'where ' || i_id_name || '=' || i_id_value || CHR(10) ||
                 'and dwsact <> ''D''';    
    
    dbms_output.put_line(l_command);    
    execute immediate l_command;
    
    l_insert := 'insert into ' || i_owner || '.' ||i_table_name || '(' || CHR(10);
    l_select := 'select' || CHR(10);            
    
    for i in (
        select column_name 
        from all_tab_columns 
        where owner = i_owner and 
              table_name = i_table_name        
        minus
        select * from sync.tech_field_names)
        
        
    loop
        l_insert := l_insert || i.column_name || ',';
        l_select := l_select || i.column_name || ',';
    end loop;    
    
    l_insert := l_insert || 'validfrom, dwsact, validto) ';
    l_select := l_select || 'validto validfrom, ''D'' dwsact, to_date(''59991231'', ''yyyymmdd'') validto ' || CHR(10) ||
                 'from ' || i_owner || '.' ||i_table_name || CHR(10) ||
                 'where '|| i_id_name || '=' || i_id_value || ' and dwsact <> ''D''';
                 
    l_command :=  l_insert || l_select;
    dbms_output.put_line(l_command);
    execute immediate l_command;
    commit;  

end;

  procedure p_insert(i_owner varchar2, i_table_name varchar2, i_id_name varchar2, i_id_value number, i_updating_columns sync.string_table, i_updating_values sync.string_table) is
  l_command varchar2(32000);
  l_insert varchar2(32000);
  l_values varchar2(32000);
  l_updating_columns sync.string_table := i_updating_columns;
  l_updating_values sync.string_table := i_updating_values;
  begin 
    
    l_insert := 'insert into ' || i_owner || '.' ||i_table_name || '(' || CHR(10);    
    l_values := 'values (' || CHR(10);
    
    for i in i_updating_columns.first..i_updating_columns.last
    loop
        l_insert := l_insert || l_updating_columns(i) || ',';
        l_values := l_values || l_updating_values(i) || ' ' ||  l_updating_columns(i) || ',';
    end loop;
    
    l_insert := l_insert || 'validfrom, dwsact, validto) ';
    l_values := l_values || 'sysdate validfrom, ''I'' dwsact, to_date(''59991231'', ''yyyymmdd'') validto)';
                 
    l_command :=  l_insert || l_values;
    dbms_output.put_line(l_command);
    execute immediate l_command;
    commit;  

end;

function contains(i_owner varchar2, i_table_name varchar2, i_id_name varchar2, i_id_value number) return boolean is
    l_cnt number;
    l_command varchar2(32000);
begin
    l_command := 
    'select count(1) from ' || i_owner || '.' || i_table_name || CHR(10) ||
    'where ' || i_id_name || '=' || i_id_value || ' and dwsact <> ''D''';
    
    dbms_output.put_line(l_command);
    execute immediate l_command into l_cnt;
    if l_cnt <> 0 then
        return true;
    else
        return false;
    end if;
end;
end;
/
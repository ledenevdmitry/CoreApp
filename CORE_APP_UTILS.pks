CREATE OR REPLACE PACKAGE SYNC.core_app_utils as 
  procedure p_update(i_owner varchar2, i_table_name varchar2, i_id_name varchar2, i_id_value number, i_updating_columns sync.string_table, i_updating_values sync.string_table); 
  procedure p_delete(i_owner varchar2, i_table_name varchar2, i_id_name varchar2, i_id_value number);
  procedure p_insert(i_owner varchar2, i_table_name varchar2, i_id_name varchar2, i_id_value number, i_updating_columns sync.string_table, i_updating_values sync.string_table);
  function contains(i_owner varchar2, i_table_name varchar2, i_id_name varchar2, i_id_value number) return boolean;
end core_app_utils;
/
import {
    TableBody,
    TableCell,
    TableRow,
    Table,
    TableHeader,
    TableHeaderCell,
    TableCellLayout,
    PresenceBadgeStatus,
    Avatar,
    Text, 
    Title1,
    makeStyles,
    shorthands,
  } from "@fluentui/react-components";
  import * as React from "react";
  import {
    FolderRegular,
    EditRegular,
    OpenRegular,
    DocumentRegular,
    PeopleRegular,
    DocumentPdfRegular,
    VideoRegular,
  } from "@fluentui/react-icons";

  import {DetailsList, SelectionMode, buildColumns } from '@fluentui/react';

  const useStyles = makeStyles({
    headerDiv: {
      textAlign: "center",
    },
    headerText: {
    },
    container:{
      ...shorthands.overflow("hidden"),
    },
    leftTable:{
      width: "24.8%",
      float: "left",
      ...shorthands.borderRight("1px", "solid", "black"),
    },
    rigthTable:{
      width: "75%",
      float: "right",
      ...shorthands.overflow("scroll"),
    },
    headerCell:{
      color: "black",
      width: "250px",
    },
    tableHeaderCell:{
      fontWeight: "bold",
    },
    rightTableCell:{
      textAlign: "left",
    },
    leftTableCell:{
      textAlign: "center",
    }
  });

const Cashflow = () => {

   const items = [
  {
    file: { label: "Meeting notes", icon: <DocumentRegular /> },
    author: { label: "Max Mustermann", status: "available" },
    lastUpdated: { label: "7h ago", timestamp: 1 },
    lastUpdate: {
      label: "You edited this",
      icon: <EditRegular />,
    },
    test1: {label: "test1"},
    test2: {label: "test2"},
    test3: {label: "test3"},
    test4: {label: "test1"},
    test5: {label: "test2"},
    test6: {label: "test3"},
    test7: {label: "test1"},
    test8: {label: "test2"},
    test9: {label: "test3"},
  },
  {
    file: { label: "Thursday presentation", icon: <FolderRegular /> },
    author: { label: "Erika Mustermann", status: "busy" },
    lastUpdated: { label: "Yesterday at 1:45 PM", timestamp: 2 },
    lastUpdate: {
      label: "You recently opened this",
      icon: <OpenRegular />,
    },
    test1: {label: "test1"},
    test2: {label: "test2"},
    test3: {label: "test3"},
    test4: {label: "test1"},
    test5: {label: "test2"},
    test6: {label: "test3"},
    test7: {label: "test1"},
    test8: {label: "test2"},
    test9: {label: "test3"},
  },
  {
    file: { label: "Training recording", icon: <VideoRegular /> },
    author: { label: "John Doe", status: "away" },
    lastUpdated: { label: "Yesterday at 1:45 PM", timestamp: 2 },
    lastUpdate: {
      label: "You recently opened this",
      icon: <OpenRegular />,
    },
    test1: {label: "test1"},
    test2: {label: "test2"},
    test3: {label: "test3"},
    test4: {label: "test1"},
    test5: {label: "test2"},
    test6: {label: "test3"},
    test7: {label: "test1"},
    test8: {label: "test2"},
    test9: {label: "test3"},
  },
  {
    file: { label: "Purchase order", icon: <DocumentPdfRegular /> },
    author: { label: "Jane Doe", status: "offline" },
    lastUpdated: { label: "Tue at 9:30 AM", timestamp: 3 },
    lastUpdate: {
      label: "You shared this in a Teams chat",
      icon: <PeopleRegular />,
    },
    test1: {label: "test1"},
    test2: {label: "test2"},
    test3: {label: "test3"},
    test4: {label: "test1"},
    test5: {label: "test2"},
    test6: {label: "test3"},
    test7: {label: "test1"},
    test8: {label: "test2"},
    test9: {label: "test3"},
  },
];
const columns = [
    { columnKey: "file", label: "File" },
    { columnKey: "author", label: "Author" },
    { columnKey: "lastUpdated", label: "Last updated" },
    { columnKey: "lastUpdate", label: "Last update" },
    { columnKey: "test1", label: "Test1"},
    { columnKey: "test2", label: "Test2"},
    { columnKey: "test3", label: "Test3"},
    { columnKey: "test4", label: "Test4"},
    { columnKey: "test5", label: "Test5"},
    { columnKey: "test6", label: "Test6"},
    { columnKey: "test7", label: "Test7"},
    { columnKey: "test8", label: "Test8"},
    { columnKey: "test9", label: "Test9"},
  ];


const months = [
  {
    date: {columnkey: "Month1", label: "20 June 2023"},
  },
  {
    date: {columnkey: "Month2", label: "20 July 2023"}
  },
  {
    date: {columnkey: "Month3", label: "20 August 2023"}
  },
  {
    date: {columnkey: "Month4", label: "20 September 2023"}
  },
]

const styles = useStyles();

return (
    <div className={styles.container}>
      <div className={styles.headerDiv}>
      <Title1 className={styles.headerText} >Cashflow</Title1>
      </div>
        <div className={styles.leftTable}>
        <Table>
            <TableHeader>
        <TableRow>
            <TableHeaderCell>
            </TableHeaderCell>
        </TableRow>
      </TableHeader>
      <TableBody>
        {columns.map((item) => (
          <TableRow className={styles.leftTableCell} key={item.columnKey}>
            <TableCell >
                {item.label}
            </TableCell>
          </TableRow>
        ))}
      </TableBody>
      </Table>
      </div>
      <div className={styles.rigthTable}>
    <Table>
      <TableHeader>
        <TableRow>
          {months.map((column) => (
            <TableHeaderCell className={styles.headerCell} key={column.date.columnkey}>
             <Text className={styles.tableHeaderCell}>{column.date.label}</Text>
            </TableHeaderCell>
          ))}
        </TableRow>
      </TableHeader>
      <TableBody>
        {columns.map((element : any) => (
          
          <TableRow key={element.columnKey}>
          {items.map((item : any, index) => (
            <TableCell className={styles.rightTableCell}>{item[element.columnKey].label}</TableCell>
          ))}
          </TableRow>
            
        ))}
      </TableBody>
    </Table>
    </div>
    </div>
  );
          
}
export default Cashflow;
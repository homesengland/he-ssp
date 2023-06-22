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
    Field,
    Input,
    InputProps,
    useTableFeatures,
    TableColumnDefinition,
    useTableSelection,
    createTableColumn,
    TableSelectionCell,
    ToggleButton,
    ToggleButtonProps,
    TableRowId,
  } from "@fluentui/react-components";
  import React, { useState, useEffect } from 'react';
  import {
    FolderRegular,
    EditRegular,
    OpenRegular,
    DocumentRegular,
    PeopleRegular,
    DocumentPdfRegular,
    VideoRegular,
  } from "@fluentui/react-icons";
  import { DatePicker, DatePickerProps } from "@fluentui/react-datepicker-compat";
  import {DetailsList, SelectionMode, buildColumns } from '@fluentui/react';

  const useStyles = makeStyles({
    headerContainer:{
      display: "flex",
      marginTop:"10px",
      marginRight: "20px",
    },
    leftHeaderDiv: {
      textAlign: "center",
      align: "left",
      width: "24.8%",
    },
    headerText: {
      marginTop: "10px",
    },
    rightHeaderDiv:{
      width: "75%",
      float: "right",
    },
    rightHeaderElement:{
      maxWidth: "300px",
      marginRight:"10px",
      float: "right",
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
      minHeight: '45px',
    }
  });

  type OpenMarketHousingCell = {
    label: string;
    value: number;
  };
  
  type MarketRentCell = {
    label: string;
    value: number;
  };
  
  type AffordableRentCell = {
    label: string;
    value: number;
  };
  
  type AffordableHomeOwnershipCell = {
    label: string;
    value: number;
  };

  type OtherUnitsCell = {
    label: string;
    value: number;
  };
  
  type Units = {
    openMarketHousing: OpenMarketHousingCell;
    affordableHomeOwnership: AffordableHomeOwnershipCell;
    marketRent: MarketRentCell;
    affordableRent: AffordableRentCell;
    otherUnits: OtherUnitsCell;
  };

  const columns: TableColumnDefinition<Units>[] = [
    createTableColumn<Units>({
      columnId: "openMarketHousing",
    }),
    createTableColumn<Units>({
      columnId: "affordableHomeOwnership",
    }),
    createTableColumn<Units>({
      columnId: "marketRent",
    }),
    createTableColumn<Units>({
      columnId: "affordableRent",
    }),
    createTableColumn<Units>({
      columnId: "otherUnits",
    }),
  ]

  const generateRandomNumber = function (max : number = 10, min : number = 0 ) : number{
    return Math.floor(min + Math.random() * (max - min))
  }

  const items: Units[] = [
    {
      openMarketHousing: { label: "Open Market Housing", value: generateRandomNumber()},
      affordableHomeOwnership: { label: "Affordable Home Ownership", value: generateRandomNumber() },
      marketRent: { label: "Market Rent", value: generateRandomNumber() },
      affordableRent: { label: "Affordable Rent",
        value: generateRandomNumber()
      },
      otherUnits: {label: "Other Units", value: generateRandomNumber()},
    },
    {
      openMarketHousing: { label: "Open Market Housing", value: generateRandomNumber()},
      affordableHomeOwnership: { label: "Affordable Home Ownership", value: generateRandomNumber() },
      marketRent: { label: "Market Rent", value: generateRandomNumber() },
      affordableRent: { label: "Affordable Rent",
        value: generateRandomNumber()
      },
      otherUnits: {label: "Other Units", value: generateRandomNumber()},
    },
    {
      openMarketHousing: { label: "Open Market Housing", value: generateRandomNumber()},
      affordableHomeOwnership: { label: "Affordable Home Ownership", value: generateRandomNumber() },
      marketRent: { label: "Market Rent", value: generateRandomNumber() },
      affordableRent: { label: "Affordable Rent",
        value: generateRandomNumber()
      },
      otherUnits: {label: "Other Units", value: generateRandomNumber()},
    },
    {
      openMarketHousing: { label: "Open Market Housing", value: generateRandomNumber()},
      affordableHomeOwnership: { label: "Affordable Home Ownership", value: generateRandomNumber() },
      marketRent: { label: "Market Rent", value: generateRandomNumber() },
      affordableRent: { label: "Affordable Rent",
        value: generateRandomNumber()
      },
      otherUnits: {label: "Other Units", value: generateRandomNumber()},
    },
    {
      openMarketHousing: { label: "Open Market Housing", value: generateRandomNumber()},
      affordableHomeOwnership: { label: "Affordable Home Ownership", value: generateRandomNumber() },
      marketRent: { label: "Market Rent", value: generateRandomNumber() },
      affordableRent: { label: "Affordable Rent",
        value: generateRandomNumber()
      },
      otherUnits: {label: "Other Units", value: generateRandomNumber()},
    },
    {
      openMarketHousing: { label: "Open Market Housing", value: generateRandomNumber()},
      affordableHomeOwnership: { label: "Affordable Home Ownership", value: generateRandomNumber() },
      marketRent: { label: "Market Rent", value: generateRandomNumber() },
      affordableRent: { label: "Affordable Rent",
        value: generateRandomNumber()
      },
      otherUnits: {label: "Other Units", value: generateRandomNumber()},
    },
    {
      openMarketHousing: { label: "Open Market Housing", value: generateRandomNumber()},
      affordableHomeOwnership: { label: "Affordable Home Ownership", value: generateRandomNumber() },
      marketRent: { label: "Market Rent", value: generateRandomNumber() },
      affordableRent: { label: "Affordable Rent",
        value: generateRandomNumber()
      },
      otherUnits: {label: "Other Units", value: generateRandomNumber()},
    },
    {
      openMarketHousing: { label: "Open Market Housing", value: generateRandomNumber()},
      affordableHomeOwnership: { label: "Affordable Home Ownership", value: generateRandomNumber() },
      marketRent: { label: "Market Rent", value: generateRandomNumber() },
      affordableRent: { label: "Affordable Rent",
        value: generateRandomNumber()
      },
      otherUnits: {label: "Other Units", value: generateRandomNumber()},
    },
  ];
  const columnsKeyAndLabels = [
      { columnKey: "openMarketHousing", label: "Open Market Housing" },
      { columnKey: "affordableHomeOwnership", label: "Affordable Home Ownership" },
      { columnKey: "marketRent", label: "Market Rent" },
      { columnKey: "affordableRent", label: "Affordable Rent" },
      { columnKey: "otherUnits", label: "Other Units"},
    ];
  const months = [
      {
        date: {columnkey: "Month1", label: "2023-06-20"}
      },
      {
        date: {columnkey: "Month2", label: "2023-07-20"}
      },
      {
        date: {columnkey: "Month3", label: "2023-08-20"}
      },
      {
        date: {columnkey: "Month4", label: "2023-09-20"}
      },
      {
        date: {columnkey: "Month5", label: "2023-10-20"}
      },
      {
        date: {columnkey: "Month6", label: "2023-11-20"}
      },
      {
        date: {columnkey: "Month7", label: "2023-12-20"}
      },
      {
        date: {columnkey: "Month8", label: "2024-01-20"}
      },
  ]
  
  const sumArray = new Array(columnsKeyAndLabels.length).fill(0)

const Cashflow = () => {

sumArray.fill(0)

const styles = useStyles();

let [searchTerm, setSearchTerm] = useState("");

let [startDate, setStartDate] = useState(new Date("1900-01-01"))

let [endDate, setEndDate] = useState(new Date("2200-12-31"))

const [toggleChecked, setToggleChecked] = useState(false);

const [toggleText, setToggleText] = useState("Show selected only")

const [allSelected, setAllSelected] = useState(false)

const renderLeftTableRows = (item : any) => {
  if(searchTerm == "" || item.columnKey.toLowerCase().includes(searchTerm.toLowerCase())){
    const selected = isRowSelected(item.columnKey);
    if((selected && toggleChecked) || !toggleChecked){
    return(
    <TableRow className={styles.leftTableCell} key={item.columnKey}
    onClick={(e: React.MouseEvent) => toggleRow(e, item.columnKey)}
      aria-selected={selected}
      appearance={ selected ? ("brand" as const) : ("none" as const)}>
            <TableSelectionCell
          checked={selected}
          checkboxIndicator={{ "aria-label": "Select row" }}
        />
      <TableCell >
          {item.label}
      </TableCell>
    </TableRow>
    )
    }
    }
}

const renderRightTableRows = (element : any) => {
  if(searchTerm == "" || element.columnKey.toLowerCase().includes(searchTerm.toLowerCase())){
    const selected = isRowSelected(element.columnKey);
    if((selected && toggleChecked) || !toggleChecked){
    return(
      <TableRow 
      key={element.columnKey}
      onClick={(e: React.MouseEvent) => toggleRow(e, element.columnKey)}
      aria-selected={selected}
      appearance={ selected ? ("brand" as const) : ("none" as const)}>
          {items.map((item : any, index) => (
            renderRightTableCells(item, index, element)
          ))}
          </TableRow>
      )
    }
  }
}

const selectAllRows = (e: React.MouseEvent) => {
  setAllSelected(!allSelected)
  columnsKeyAndLabels.map((element) => (
    selectSingleRow(e, element)
  ))
  
}

const selectSingleRow= (e: React.MouseEvent, element : any) => {
  if( (!allSelected && !isRowSelected(element.columnKey)) || (allSelected && isRowSelected(element.columnKey)) ){
    toggleRow(e, element.columnKey)
  }
}

const renderRightTableCells = (item : any, index : number, element : any) => {
  const date = new Date(months[index].date.label)
  if((startDate == null && endDate == null) || (date >= startDate && date <= endDate) 
      || (startDate == null && date <= endDate) || ( endDate == null && date >= startDate) ){
        sumArray[index] += item[element.columnKey].value
    return (
      <TableCell className={styles.rightTableCell}>{item[element.columnKey].value}</TableCell>
    )
  }
}

const renderMonthsColumns = (column : any) => {
  var date = new Date(column.date.label)
  if((startDate == null && endDate == null) || (date >= startDate && date <= endDate) 
      || (startDate == null && date <= endDate) || ( endDate == null && date >= startDate) ){
    return(
      <TableHeaderCell className={styles.headerCell} key={column.date.columnkey}>
        <Text className={styles.tableHeaderCell}>{column.date.label}</Text>
      </TableHeaderCell>) 
    }
    
  }

  const renderSumRow = (element : any) => {
    return(
      <TableCell className={styles.rightTableCell}>{element}</TableCell>
    )
  }

  const handleToggleChange = () => {
    setToggleChecked(!toggleChecked)
    setToggleText(!toggleChecked ? "Show all" : "Show selected only")
  }


useEffect(() => {
  // Update the document title using the browser API
});

  const [selectedRows, setSelectedRows] = React.useState(
    () => new Set<TableRowId>([0, 1])
  );

const {
  getRows,
  selection: {
    allRowsSelected,
    someRowsSelected,
    toggleAllRows,
    toggleRow,
    isRowSelected,
  },
} = useTableFeatures(
  {
    columns,
    items,
  },
  [
    useTableSelection({
      selectionMode: "multiselect",
      selectedItems: selectedRows,
      onSelectionChange: (e, data) => setSelectedRows(data.selectedItems),
    }),
  ]
);


const toggleAllKeydown = React.useCallback(
  (e: React.KeyboardEvent<HTMLDivElement>) => {
    if (e.key === " ") {
      toggleAllRows(e);
      e.preventDefault();
    }
  },
  [toggleAllRows]
);

return (
    <div className={styles.container}>
      <div className={styles.headerContainer}>
      <div className={styles.leftHeaderDiv}>
      <Title1 className={styles.headerText} >Cashflow</Title1>
      </div>
      <div className={styles.rightHeaderDiv}>
        <Field className={styles.rightHeaderElement}>
        <label>Filter by row name</label>
          <Input onChange={(value) => setSearchTerm(searchTerm = value.target.value)}/>
          </Field>
        <Field className={styles.rightHeaderElement}>
        <label>Select end date</label>
          <DatePicker 
            onSelectDate={(date) => setEndDate(endDate = date ?? new Date("2200-12-31"))} 
            placeholder="Select end date" />
          </Field>
        <Field className={styles.rightHeaderElement}>
          <label>Select start date</label>
          <DatePicker 
            onSelectDate={(date) => setStartDate(startDate = date ?? new Date("1900-01-01"))} 
            placeholder="Select start date" />
            </Field>
      </div>
      </div>
        <div className={styles.leftTable}>
        <Table>
            <TableHeader>
        <TableRow>
        <TableSelectionCell
            checked={
              allSelected
            }
            onClick={(e: React.MouseEvent) => selectAllRows(e)}
            onKeyDown={toggleAllKeydown}
            checkboxIndicator={{ "aria-label": "Select all rows " }}
          />
        <ToggleButton onClick={() => handleToggleChange()}>{toggleText}</ToggleButton>
            
        </TableRow>
      </TableHeader>
      <TableBody>
        {columnsKeyAndLabels.map((item) => (
          renderLeftTableRows(item)
        ))}
        <TableRow className={styles.leftTableCell} key="sumTableRow">
          <TableCell></TableCell>
          <TableCell>Total housing completions</TableCell></TableRow>
      </TableBody>
      </Table>
      </div>
      <div className={styles.rigthTable}>
    <Table>
      <TableHeader>
        <TableRow>
          {months.map((column) => (
            renderMonthsColumns(column)
          ))}
        </TableRow>
      </TableHeader>
      <TableBody>
        {columnsKeyAndLabels.map((element) => (
          renderRightTableRows(element)
        ))}
        <TableRow key="SumColumn">
        {sumArray.map((element) => (
          renderSumRow(element)
        ))}
        </TableRow>
      </TableBody>
    </Table>
    </div>
    </div>
  );
          
}
export default Cashflow;
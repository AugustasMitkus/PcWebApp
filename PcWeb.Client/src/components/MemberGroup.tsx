import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import '../index.css';
import Table from "react-bootstrap/Table"

type Member = {
    id: number;
    firstName: string;
    lastName: string;
    debtSum: number;
}

const MemberGroup : React.FC = () => {
    const { groupId } = useParams();
    const [members, setMembers] = useState<Member[]>([]);
    const [currentMembers, setCurrentMembers] = useState<Member[]>([]);
    const [groupName, setGroupName] = useState<string>("");
    const [pointOfView, setPointOfView] = useState<number>(0);
    const [firstName, setFirstName] = useState<string>("");
    const [lastName, setLastName] = useState<string>("");
    const navigate = useNavigate();
    useEffect(() => {
            //fetches a group and assigns its title/name
            const fetchGroup = async () => {
                try {
                    const response = await fetch(`http://localhost:5109/groups/${groupId}`)
                    

                    if (response.ok) {
                        const data = await response.json();
                        setGroupName(data.name);

                    }
    
                    else {
                    const errorData = await response.json();
                    console.error(errorData.error)
                }
                }
                catch (error:any){
                    console.error("Error:", error.message);
                }
            };

            const fetchMembers = async () => {
                try {
                    //fetches members of the group alongside their general debt amount (sum)
                    const response = await fetch(`http://localhost:5109/members/group/${groupId}`)
                    if (response.ok) {
                        const data = await response.json();
                        setMembers(data);
                        setCurrentMembers(data);
                    }
                }
                catch (error:any){
                    console.error("Error:", error.message);
                } 
            };
        fetchGroup();
        fetchMembers();
    }, [groupId]);

    const handleViewChange = async (value: number) => {
        setPointOfView(value);
        try {
            if (value === 0) {
                setCurrentMembers(members);
            } else {
                //when a view has been picked it shows debts according to that choice (if "General" has been picked it shows general sums of their debts)
                const response = await fetch(`http://localhost:5109/debts/${value}`);
                if (response.ok) {
                    const data = await response.json();
                    console.log(data);
                    setCurrentMembers(data);
                }
            }
        } 
        catch (error: any) {
            console.error("Error:", error.message);
        }
    };
    const handleAddMember = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        try {
            //add a member
            const response = await fetch("http://localhost:5109/members/", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ firstName: firstName, lastName: lastName, groupId: groupId})
            });

            if (response.ok) {
                const data = await response.json();
                setFirstName("");
                setLastName("");
                //once the member has been added it creates debts with the rest of the members
                const debtResponse = await fetch(`http://localhost:5109/debts/${data}`, {
                    method: "POST",
                    headers: {
                    "Content-Type": "application/json"
                    },
                    body: JSON.stringify({ groupId: groupId})
                });
                if (debtResponse.ok){
                    const debtData = await debtResponse.json();
                    console.log(debtData);
                }
                window.location.reload();
            }
            else {
                const errorData = await response.json();
                console.error(errorData.error)
            }

        }
        catch (error: any) {
            console.error("Error:", error.message);
        }
    }
    const handleDeleteMember = async () => {
        try {
            if (currentMembers.every(member => member.debtSum !== 0)){
                throw alert("There are unsettled debts for the current member!")
            }
            //deletes the member but only if all the debts have been settled
            const response = await fetch(`http://localhost:5109/members/${pointOfView}`, {
                method: "DELETE"
            });
            if (response.ok) {
                alert("Member has been deleted");
                window.location.reload();
            } 
            else {
            const errorData = await response.json();
            console.error(errorData.error);
            }
        } 
        catch (error: any) {
            console.error("Error:", error.message);
        }
    }

    const handleTransactionRedirect = (groupId: number) => {
        navigate(`/transaction/${groupId}`);
    }

    return (
        <main className="container">
            <button onClick={() => navigate("/")} className="groupBtn">
                    Back
            </button>
            <center>
                <h2 className="title">{groupName}</h2>
                <div className="membersTop">
                    <label htmlFor="view">View: </label>
                    <select
                        name="view"
                        id="view"
                        value={pointOfView}
                        onChange={(e) => handleViewChange(Number(e.target.value))}
                        style={{ width: "100%", height: "40px", padding: "5px", margin: "20px"}}
                    >
                        <option value={0}>General</option>
                        {members.map((member) => (
                            <option key={member.id} value={member.id}>{member.firstName} {member.lastName}</option>
                        ))}
                    </select>
                    <button type="button" className="groupBtn" name="transaction" onClick={() => handleTransactionRedirect(Number(groupId))}>Make a transaction</button>
                </div>
                <Table striped bordered hover>
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>First Name</th>
                            <th>Last Name</th>
                            <th>Debt</th>
                            <th>Settle</th>
                        </tr>
                    </thead>
                    <tbody>
                        {currentMembers.map((member) => (
                            <tr key={member.id}>
                                <td>{member.id}</td>
                                <td>{member.firstName}</td>
                                <td>{member.lastName}</td>
                                <td>{member.debtSum}</td>
                                {pointOfView !== 0 && member.debtSum !== 0 &&(
                                    <td style={{ cursor: "pointer" }}>Settle</td>
                                )} 
                            </tr>
                        ))}
                    </tbody>
                </Table>
            <form onSubmit={handleAddMember}>
                <label htmlFor="firstName">First name:</label>
                <input required type="text" 
                    style={{margin: "10px"}}
                    name="firstName"
                    maxLength={20}
                    value={firstName}
                    onChange={(e) => setFirstName(e.target.value)}/>
                <label htmlFor="lastName">Last name:</label>
                <input required type="text" 
                    style={{margin: "10px"}}
                    name="lastName"
                    maxLength={20}
                    value={lastName}
                    onChange={(e) => setLastName(e.target.value)}/>
                <button type="submit" className="groupBtn">Add member</button>
            </form>
            {pointOfView !== 0 && (
                <button className="deleteBtn" type="button" onClick={handleDeleteMember}>Delete Current Member</button>
            )}
            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>Type</th>
                        <th>Name</th>
                        <th>Amount</th>
                    </tr>
                </thead>
            </Table>
            </center>
        </main>
    )
}

export default MemberGroup
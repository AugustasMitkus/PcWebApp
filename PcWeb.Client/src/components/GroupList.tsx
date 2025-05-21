import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import '../index.css';
import Table from "react-bootstrap/Table"

type Group = {
    id: number;
    name: string;
}
const GroupList: React.FC = () => {
    const [groupName, setGroupName] = useState<string>("");
    const [memberGroups, setMemberGroups] = useState<Group[]>([]);
    const navigate = useNavigate();

    useEffect(() => {
        //fetches groups from the database (...just as the name suggests)
        const fetchGroups = async () => {
            try {
                const response = await fetch("http://localhost:5109/groups/")
                if (response.ok) {
                    const data = await response.json();
                    setMemberGroups(data);
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
    fetchGroups();
}, []);
    //sends required data to the api so a group would be created
    const handleCreateGroup = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        try {
            const response = await fetch("http://localhost:5109/groups/", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ Name: groupName })
            });

            if (response.ok) {
                const data = await response.json();
                setGroupName("");
                console.log(data);
                const groupUpdate = await fetch("http://localhost:5109/groups/")
                const groupData = await groupUpdate.json();
                setMemberGroups(groupData);
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

    const handleGroupClick = (groupId: number) => {
        navigate(`/group/${groupId}`);
    }

    return (
        <main className="container">
            <center>
                <h2 className="title">List of groups</h2>
                <form className="addGroup" onSubmit={handleCreateGroup}>
                    <label htmlFor="Name">Enter the name of the group: </label>
                    <input
                        required
                        type="text"
                        style={{margin: "10px"}}
                        name="Name"
                        maxLength={20}
                        value={groupName}
                        onChange={(e) => setGroupName(e.target.value)}
                    />
                    <button className="groupBtn" type="submit" name="create">Create a group</button>
                </form>
                <Table striped bordered hover>
                    <thead>
                        <tr>
                            <th style={{width: "20px"}}>Group ID</th>
                            <th>Name</th>
                        </tr>
                    </thead>
                    <tbody>
                        {memberGroups.map((memberGroup) => (
                            <tr key={memberGroup.id}>
                                <td>{memberGroup.id}</td>
                                <td style={{ cursor: "pointer" }} onClick={() => handleGroupClick(memberGroup.id)}>{memberGroup.name}</td>
                            </tr>
                        ))}
                    </tbody>
                </Table>
            </center>    
        </main>
    )
}

export default GroupList
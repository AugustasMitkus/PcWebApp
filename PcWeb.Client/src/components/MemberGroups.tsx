import React, { useEffect, useState } from "react";
import '../index.css';

const MemberGroups: React.FC = () => {
    const [groupName, setGroupName] = useState<string>("");
    useEffect(() => {
        
    })

    const handleCreateGroup = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        try {
            const response = await fetch("http://localhost:5109/groups/", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ groupName })
            });

            if (response.ok) {
                const data = await response.json();
                setGroupName("");
                console.log(data.message);
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

    return (
        <main className="container">
            <center>
                <h2 className="title">List of groups</h2>
                <form className="addGroup" onSubmit={handleCreateGroup}>
                    <input
                        type="text"
                        name="Name"
                        maxLength={20}
                        value={groupName}
                        onChange={(e) => setGroupName(e.target.value)}
                    />
                    <button className="groupBtn" type="submit" name="create">Create a group</button>
                </form>
            </center>    
        </main>
    )
}

export default MemberGroups